#region usings

using MediaService.BLL.DTO;
using MediaService.BLL.DTO.Enums;
using MediaService.BLL.Interfaces;
using MediaService.BLL.Models;
using MediaService.BLL.Models.Enums;
using MediaService.BLL.Models.QueueMessages;
using MediaService.DAL.Accessors.Enums;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, FileEntry, Guid>, IFileService
    {
        #region Properties

        private IBlobStorage Storage { get; }

        private IQueueStorage Queue { get; }

        #endregion

        #region Constructor

        public FileService(IUnitOfWork uow, IBlobStorage storage, IQueueStorage queue) : base(uow)
        {
            Storage = storage;
            Queue = queue;
            Repository = uow.Files;
        }

        #endregion

        #region Methods

        public async Task<bool> ExistAsync(string name, Guid parentId)
        {
            return await Context.Files.AnyAsync(f => f.Name == name && f.ParentId == parentId);
        }

        #region Select Methods

        public async Task<IEnumerable<FileEntryDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(await Context.Files.GetDataAsync(f => f.ParentId == id));
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
            var dirs = GetQuery(id, name, parentId, created, downloaded, modified, ownerId);
            return await Task.Run(() => DtoMapper.Map<IEnumerable<FileEntryDto>>(dirs.AsParallel().ToList()));
        }

        public string GetLinkToZip(string fileName)
        {
            return Storage.GetDirectLinkToBlob(fileName, DateTimeOffset.Now.AddHours(1), SharedAccessBlobPermissions.Read);
        }

        public async Task<string> GetPublicLinkToFileAsync(Guid fileId, DateTimeOffset expiryTime)
        {
            var fileEntry = await Context.Files.FindByKeyAsync(fileId);

            if (fileEntry == null)
            {
                throw new InvalidDataException("Can't find this file in database");
            }

            return Storage.GetDirectLinkToBlob($"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}", expiryTime, SharedAccessBlobPermissions.Read);
        }

        public async Task<string> GetLinkToFileThumbnailAsync(Guid fileId)
        {
            var fileEntry = await Context.Files.FindByKeyAsync(fileId);

            if (fileEntry == null)
            {
                throw new InvalidDataException("Can't find this file in database");
            }

            return Storage.GetDirectLinkToBlob($"thumbnail-{fileEntry.Id}.png", DateTimeOffset.Now.AddMinutes(5), SharedAccessBlobPermissions.Read);
        }

        public async Task<IEnumerable<FileEntryDto>> SearchFilesAsync(Guid parentId, SearchType searchType, string searchValue)
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(await SearchFilesHelperAsync(parentId, searchType, searchValue));
        }

        #endregion

        #region Create Methods

        public async Task AddRangeAsync(IEnumerable<FileEntryDto> filesDto, Guid folderId)
        {
            var parentDir = await Context.Directories.FindByKeyAsync(folderId);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;

            var filesNames = new List<string>();

            foreach (var fileDto in filesDto)
            {
                var fileEntry = DtoMapper.Map<FileEntry>(fileDto);
                var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

                fileEntry.Downloaded = fileEntry.Created = fileEntry.Modified = DateTime.Now;
                fileEntry.Owner = parentDir.Owner;
                fileEntry.Parent = parentDir;
               
                await Context.Files.AddAsync(fileEntry);
                await Context.SaveChangesAsync();

                var fileName = $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}";

                await Storage.UploadAsync(fileDto.FileStream, fileName, mimeType);

                filesNames.Add(fileName);
            }

            //var messageInfo = new ThumbnailMessageInfo { OperationType = OperationType.GenerateThumbnail, FilesNames = filesNames};

            //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.GenerateThumbnails);
            await GenerateThumbnailsToFilesAsync(filesNames);
        }

        public override void Add(FileEntryDto item)
        {
            if (!item.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't create file without any parent directory");
            }

            var parentDir = Context.Directories.FindByKey(item.ParentId.Value);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;

            var fileEntry = DtoMapper.Map<FileEntry>(item);
            var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

            fileEntry.Downloaded = fileEntry.Created = fileEntry.Modified = DateTime.Now;
            fileEntry.Owner = parentDir.Owner;
            fileEntry.Parent = parentDir;
            Context.Files.Add(fileEntry);
            Context.SaveChanges();
            Storage.Upload(item.FileStream, $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}", mimeType);
        }

        public override async Task AddAsync(FileEntryDto item)
        {
            if (!item.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't create file without any parent directory");
            }

            var parentDir = await Context.Directories.FindByKeyAsync(item.ParentId.Value);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;

            var fileEntry = DtoMapper.Map<FileEntry>(item);
            var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

            fileEntry.Downloaded = fileEntry.Created = fileEntry.Modified = DateTime.Now;
            fileEntry.Owner = parentDir.Owner;
            fileEntry.Parent = parentDir;
            await Context.Files.AddAsync(fileEntry);
            await Context.SaveChangesAsync();
            await Storage.UploadAsync(item.FileStream, $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}", mimeType);
        }

        public async Task AddTagAsync(Guid fileId, string tagName)
        {
            var fileEntry = await Context.Files.FindByKeyAsync(fileId);

            var tagEntry = (await Context.Tags.GetDataAsync(t => t.Name == tagName)).FirstOrDefault();
            if (tagEntry == null)
            {
                tagEntry = new Tag { Name = tagName };
                tagEntry.FileEntries.Add(fileEntry);
                await Context.Tags.AddAsync(tagEntry);
            }
            else if (fileEntry.Tags.All(t => t.Id != tagEntry.Id))
            {
                tagEntry.FileEntries.Add(fileEntry);
                await Context.Tags.UpdateAsync(tagEntry);
            }
            //fileEntry.Tags.Add(tagEntry);
            //await Context.Files.UpdateAsync(fileEntry);
            await Context.SaveChangesAsync();
        }

        public async Task GenerateThumbnailsToFilesAsync(IEnumerable<string> filesNames)
        {
            foreach (var fileName in filesNames)
            {
                (var blobStream, var blobExist) = await Storage.DownloadAsync(fileName);

                if (blobExist)
                {
                    var thumb = Image.FromStream(blobStream).GetThumbnailImage(250, 180, () => false, IntPtr.Zero);
                    var thumbnailStream = new MemoryStream();

                    thumb.Save(thumbnailStream, ImageFormat.Png);
                    thumbnailStream.Position = 0;
                    await Storage.UploadAsync(thumbnailStream, $"thumbnail-{Path.GetFileNameWithoutExtension(fileName)}.png", "image/png");
                }
            }
        }

        #endregion

        #region Update Methods

        public async Task RenameAsync(FileEntryDto editedFileEntryDto)
        {
            var currFileEntry = await Context.Files.FindByKeyAsync(editedFileEntryDto.Id);

            if (currFileEntry == null)
            {
                throw new InvalidDataException("Can't find user's file with this Id in database");
            }
            if (currFileEntry.Parent != null)
            {
                currFileEntry.Parent.Modified = DateTime.Now;
            }
            currFileEntry.Modified = DateTime.Now;
            currFileEntry.Name = editedFileEntryDto.Name;

            await Context.Files.UpdateAsync(currFileEntry);
            await Context.SaveChangesAsync();
        }

        #endregion

        #region Delete Methods

        public async Task DeleteAsync(Guid entryId)
        {
            var currFileEntry = await Context.Files.FindByKeyAsync(entryId);

            if (currFileEntry == null)
            {
                throw new InvalidDataException("Can't find user's file with this Id in database");
            }
            var x = currFileEntry.Tags.ToList();
            for (int i = 0; i < x.Count; i++)
            {
                if (x[i].FileEntries.Count == 1)
                {
                    await Context.Tags.RemoveAsync(x[i]);
                }

            }

            var tagsToDelete = new List<Tag>();

            foreach (var tag in currFileEntry.Tags)
            {
                if (tag.FileEntries.Count == 1)
                {
                    tagsToDelete.Add(tag);
                }
            }

            await Context.Tags.RemoveRangeAsync(tagsToDelete);
            await Context.Files.RemoveAsync(currFileEntry);
            await Storage.DeleteAsync($"{currFileEntry.Id}{Path.GetExtension(currFileEntry.Name)}");

            await Context.SaveChangesAsync();
        }

        #endregion

        #region Download Methods

        public async Task DownloadWithJobAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            //var messageInfo = new DownloadMessageInfo { OperationType = OperationType.DownloadFiles, EntriesIds = filesIds.ToList(), ZipId = zipId };

            //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.Download);

            await DownloadAsync(filesIds, zipId);
        }

        public async Task<Stream> DownloadAsync(Guid fileId)
        {
            var fileEntry = await Context.Files.FindByKeyAsync(fileId);

            if (fileEntry == null)
            {
                throw new InvalidDataException("There is no such file in database to download");
            }

            (var blobStream, _) = await Storage.DownloadAsync(fileEntry.Name, fileEntry.Size);

            return blobStream;
        }

        public async Task DownloadAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            var filesEntries = (await Context.Files.GetDataAsync(f => filesIds.Contains(f.Id))).ToList();

            if (filesEntries == null || !filesEntries.Any())
            {
                throw new InvalidDataException("There is no files in database to download");
            }

            var zipName = $"{zipId}.zip";

            using (var fs = new FileStream(zipName, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create, false))
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
            File.Delete(zipName);
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
            var dirs = Context.Files.GetQuery();
            if (id.HasValue)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Id == id.Value));
            }
            if (name != null)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Name == name));
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.ParentId == parentId));
            }
            if (created.HasValue)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Created == created));
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Downloaded == downloaded));
            }
            if (modified.HasValue)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Modified == modified));
            }
            if (ownerId != null)
            {
                dirs = dirs.Intersect(Context.Files.GetQuery(f => f.Owner.Id == ownerId));
            }

            return dirs;
        }

        private async Task<IEnumerable<FileEntry>> SearchFilesHelperAsync(Guid parentId, SearchType searchType, string searchValue)
        {
            var childDirs = await Context.Directories.GetDataAsync(f => f.ParentId == parentId);

            IEnumerable<FileEntry> result;
            switch (searchType)
            {
                case SearchType.ByName:
                    result = await Context.Files.GetDataAsync(f => f.ParentId == parentId && f.Name == searchValue);
                    break;
                case SearchType.ByTag:
                    result = await Context.Files.GetDataAsync(f =>
                        f.ParentId == parentId && f.Tags.Any(t => t.Name == searchValue));
                    break;
                case SearchType.None:
                    result = await Context.Files.GetDataAsync(f => f.ParentId == parentId);
                    break;
                default:
                    return Enumerable.Empty<FileEntry>();
            }

            foreach (var directoryEntry in childDirs)
            {
                result = result.Concat(await SearchFilesHelperAsync(directoryEntry.Id, searchType, searchValue));
            }

            return result;
        }

        #endregion

        #endregion
    }
}