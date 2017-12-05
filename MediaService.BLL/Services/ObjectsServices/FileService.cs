#region usings

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, FileEntry, Guid>, IFileService
    {
        #region Constructor

        public FileService(IUnitOfWork uow, IBlobStorage storage, IQueueStorage queue) : base(uow)
        {
            Storage = storage;
            Queue = queue;
            Repository = uow.Files;
        }

        #endregion

        #region Properties

        private IBlobStorage Storage { get; }

        private IQueueStorage Queue { get; }

        #endregion

        #region Methods

        public async Task<bool> ExistAsync(string name, Guid parentId)
        {
            return await Context.Files.AnyAsync(f => f.Name == name && f.ParentId == parentId);
        }

        #region Select Methods

        public async Task<IEnumerable<FileEntryDto>> GetByParentIdAsync(Guid id)
        {
            return await Task.Run(() =>
                DtoMapper.Map<IEnumerable<FileEntryDto>>(
                    Context.Files.GetQuery(f => f.ParentId == id).AsEnumerable()));
        }

        public async Task<IEnumerable<FileEntryDto>> GetByAsync(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            string ownerId = null
        )
        {
            return await Task.Run(() =>
            {
                var dirs = GetQuery(id, name, parentId, created, downloaded, modified, ownerId);
                return DtoMapper.Map<IEnumerable<FileEntryDto>>(dirs.AsParallel().ToList());
            });
        }

        public string GetLinkToZip(string fileName)
        {
            return Storage.GetDirectLinkToBlob(
                fileName,
                DateTimeOffset.Now.AddHours(1),
                SharedAccessBlobPermissions.Read
                );
        }

        public async Task<string> GetPublicLinkToFileAsync(Guid fileId, DateTimeOffset expiryTime)
        {
            return await Task.Run(() =>
            {
                var fileEntry = Context.Files.FindByKey(fileId)
                                ?? throw new InvalidDataException("Can't find this file in database");

                return Storage.GetDirectLinkToBlob(
                    $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}",
                    expiryTime,
                    SharedAccessBlobPermissions.Read
                );
            });
        }

        public async Task<string> GetLinkToFileThumbnailAsync(Guid fileId)
        {
            return await Task.Run(() =>
            {
                var fileEntry = Context.Files.FindByKeyAsync(fileId)
                                ?? throw new InvalidDataException("Can't find this file in database");

                return Storage.GetDirectLinkToBlob(
                    $"thumbnail-{fileEntry.Id}.png",
                    DateTimeOffset.Now.AddMinutes(5),
                    SharedAccessBlobPermissions.Read
                );
            });
        }

        public async Task<IEnumerable<FileEntryDto>> SearchFilesAsync(
            Guid parentId,
            SearchType searchType,
            string searchValue
            )
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(
                await SearchFilesHelperAsync(parentId, searchType, searchValue));
        }

        #endregion

        #region Create Methods

        public async Task AddRangeAsync(IEnumerable<FileEntryDto> filesDto, Guid folderId)
        {
            await Task.Run(async () =>
            {
                var parentDir = Context.Directories.FindByKey(folderId)
                                ?? throw new InvalidDataException(
                                    "Can't find parent folder user with this Id in database");

                var filesNames = new ConcurrentBag<string>();

                foreach(var fileDto in filesDto)
                {
                    var fileEntry = DtoMapper.Map<FileEntry>(fileDto);
                    var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

                    fileEntry.Downloaded = fileEntry.Created = fileEntry.Modified = DateTime.Now;
                    fileEntry.Owner = parentDir.Owner;
                    fileEntry.Parent = parentDir;

                    Context.Files.Add(fileEntry);
                    Context.SaveChanges();

                    var fileName = $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}";

                    Storage.Upload(fileDto.FileStream, fileName, mimeType);

                    filesNames.Add(fileName);
                }
                parentDir.Modified = DateTime.Now;

                //var messageInfo = new ThumbnailMessageInfo { OperationType = OperationType.GenerateThumbnail, FilesNames = filesNames};

                //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.GenerateThumbnails);
                await GenerateThumbnailsToFilesAsync(filesNames);
            });
        }

        public override void Add(FileEntryDto item)
        {
            if (!item.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't create file without any parent directory");
            }

            var parentDir = Context.Directories.FindByKey(item.ParentId.Value)
                 ?? throw new InvalidDataException("Can't find parent folder user with this Id in database");

            var fileEntry = DtoMapper.Map<FileEntry>(item);
            var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

            fileEntry.Downloaded = fileEntry.Created = fileEntry.Modified = DateTime.Now;
            fileEntry.Owner = parentDir.Owner;
            fileEntry.Parent = parentDir;

            //todo: Add transaction security
            try
            {
                parentDir.Modified = DateTime.Now;
                Context.Files.Add(fileEntry);
                Context.SaveChanges();

                Storage.Upload(item.FileStream, $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}", mimeType);
            }
            catch (Exception e)
            {
                throw new ExternalException(e.Message, e);
            }
        }

        public override async Task AddAsync(FileEntryDto item)
        {
            await Task.Run(() => Add(item));
        }

        public async Task AddTagAsync(Guid fileId, string tagName)
        {
            var fileEntry = await Context.Files.FindByKeyAsync(fileId);

            var tagEntry = Context.Tags.GetQuery(t => t.Name == tagName).FirstOrDefault();

            if (tagEntry == null)
            {
                tagEntry = new Tag {Name = tagName};
                tagEntry.FileEntries.Add(fileEntry);
                await Context.Tags.AddAsync(tagEntry);
            }
            else if (fileEntry.Tags.All(t => t.Id != tagEntry.Id))
            {
                tagEntry.FileEntries.Add(fileEntry);
                await Context.Tags.UpdateAsync(tagEntry);
            }

            await Context.SaveChangesAsync();
        }

        public async Task GenerateThumbnailsToFilesAsync(IEnumerable<string> filesNames)
        {
            foreach (var fileName in filesNames)
            {
                (var blobStream, _) = await Storage.DownloadAsync(fileName);

                if (blobStream != null)
                {
                    var thumb = Image.FromStream(blobStream).GetThumbnailImage(250, 180, () => false, IntPtr.Zero);
                    var thumbnailStream = new MemoryStream();

                    thumb.Save(thumbnailStream, ImageFormat.Png);
                    thumbnailStream.Position = 0;
                    await Storage.UploadAsync(thumbnailStream,
                        $"thumbnail-{Path.GetFileNameWithoutExtension(fileName)}.png", "image/png");
                }
            }
        }

        #endregion

        #region Update Methods

        public async Task RenameAsync(FileEntryDto editedFileEntryDto)
        {
            await Task.Run(() =>
                {
                    var currFileEntry = Context.Files.FindByKey(editedFileEntryDto.Id)
                                        ?? throw new InvalidDataException(
                                            "Can't find user's file with this Id in database");

                    if (currFileEntry.Parent != null)
                    {
                        currFileEntry.Parent.Modified = DateTime.Now;
                    }
                    currFileEntry.Modified = DateTime.Now;
                    currFileEntry.Name = editedFileEntryDto.Name;

                    Context.Files.Update(currFileEntry);
                    Context.SaveChanges();
                }
            );
        }

        #endregion

        #region Delete Methods

        public async Task DeleteAsync(Guid entryId)
        {
            var currFileEntry = Context.Files.FindByKey(entryId)
                                ?? throw new InvalidDataException(
                                    "Can't find user's file with this Id in database");

            Context.Tags.RemoveRange(currFileEntry.Tags.Where(tag => tag.FileEntries.Count == 1));

            var t1 = Task.Run(() =>
            {
                Context.Files.Remove(currFileEntry);
                Context.SaveChanges();
            });

            var t2 = Storage.DeleteAsync($"{currFileEntry.Id}{Path.GetExtension(currFileEntry.Name)}");

            try
            {
                await t1;
                await t2;
                //Task.WaitAll(t1, t2);
            }
            catch (AggregateException ae)
            {
                var message = t1.IsFaulted && t2.IsFaulted
                    ? "Error while deleting file entry from db and deleting file from storage"
                    : t1.IsFaulted
                        ? "Error while deleting file entry from db"
                        : "Error while deleting file from storage";
                throw new DbUpdateException(message, ae);
            }
        }

        #endregion

        #region Download Methods

        public async Task DownloadWithJobAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            //var messageInfo = new DownloadMessageInfo { OperationType = OperationType.DownloadFiles, EntriesIds = filesIds.ToList(), ZipId = zipId };

            //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.Download);

            await DownloadAsync(filesIds, zipId);
        }

        public async Task<(Stream blobStream, string contentType)> DownloadAsync(Guid fileId)
        {
            return await Task.Run(() =>
                {
                    var fileEntry = Context.Files.FindByKey(fileId)
                                    ??
                                    throw new InvalidDataException("There is no such file in database to download");

                    return Storage.DownloadAsync(fileEntry.Name, fileEntry.Size);
                }
            );
        }

        public async Task DownloadAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            var filesEntries = Context.Files.GetQuery(f => filesIds.Contains(f.Id)).AsParallel().ToList();

            if (filesEntries == null || !filesEntries.Any())
            {
                throw new InvalidDataException("There is no files in database to download");
            }

            var zipName = $"{zipId}.zip";

            using (var fs = new FileStream(zipName, FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Create, false))
                {
                    foreach (var fileEntry in filesEntries)
                    {
                        var blobName = $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}";
                        if (await Storage.BlobExistAsync(blobName))
                        {
                            var zipEntry = archive.CreateEntry(fileEntry.Name);
                            using (var zipEntryStream = zipEntry.Open())
                            {
                                await Storage.DownloadAsync(zipEntryStream, blobName, fileEntry.Size);
                            }
                        }
                    }
                }
                fs.Position = 0;
                await Storage.UploadFileInBlocksAsync(fs, zipName, "application/zip");
            }

            try
            {
                File.Delete(zipName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Help Methods

        private IQueryable<FileEntry> GetQuery(
            Guid? id,
            string name,
            Guid? parentId,
            DateTime? created,
            DateTime? downloaded,
            DateTime? modified,
            string ownerId
        )
        {
            var files = Context.Files.GetQuery();

            if (id.HasValue)
            {
                files = files.Where(f => f.Id == id.Value);
            }
            if (name != null)
            {
                files = files.Where(f => f.Name == name);
            }
            if (parentId.HasValue)
            {
                files = files.Where(f => f.ParentId == parentId);
            }
            if (created.HasValue)
            {
                files = files.Where(f => f.Created == created);
            }
            if (downloaded.HasValue)
            {
                files = files.Where(f => f.Downloaded == downloaded);
            }
            if (modified.HasValue)
            {
                files = files.Where(f => f.Modified == modified);
            }
            if (ownerId != null)
            {
                files = files.Where(f => f.Owner.Id == ownerId);
            }

            return files;
        }

        private async Task<IEnumerable<FileEntry>> SearchFilesHelperAsync(
            Guid parentId,
            SearchType searchType,
            string searchValue
            )
        {
            var result = Context.Files.GetQuery(f => f.ParentId == parentId);

            switch (searchType)
            {
                case SearchType.ByName:
                    result = result.Where(f => f.Name == searchValue);
                    break;
                case SearchType.ByTag:
                    result = result.Where(f => f.Tags.Any(t => t.Name == searchValue));
                    break;
                default:
                    return Enumerable.Empty<FileEntry>();
            }

            var childDirIds = Context.Directories
                .GetQuery(d => d.ParentId == parentId)
                .Select(d => d.Id);

            foreach (var dirId in childDirIds)
            {
                result = result.Concat(await SearchFilesHelperAsync(dirId, searchType, searchValue));
            }

            return result;
        }

        #endregion

        #endregion
    }
}