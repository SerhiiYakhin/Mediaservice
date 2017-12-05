#region usings

using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class DirectoryService : Service<DirectoryEntryDto, DirectoryEntry, Guid>, IDirectoryService
    {
        #region Constructor

        public DirectoryService(IUnitOfWork uow, IBlobStorage storage, IQueueStorage queue) : base(uow)
        {
            Storage = storage;
            Queue = queue;
            Repository = uow.Directories;
        }

        #endregion

        #region Properties

        private IBlobStorage Storage { get; }

        private IQueueStorage Queue { get; }

        #endregion

        #region Methods

        public async Task<bool> ExistAsync(string name, Guid parentId)
        {
            return await Context.Directories.AnyAsync(d => d.Name == name && d.ParentId == parentId);
        }

        #region Select Methods

        public async Task<IEnumerable<DirectoryEntryDto>> GetByParentIdAsync(Guid id)
        {
            return await Task.Run(() =>
                DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(
                    Context.Directories.GetQuery(d => d.ParentId == id).AsEnumerable())
            );
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByAsync(
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
                    return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(dirs.AsEnumerable());
                }
            );
        }

        /// <exception cref="InvalidDataException">
        ///     Thrown when user with given Id or his root folder doesn't exist in the Database
        /// </exception>
        public async Task<DirectoryEntryDto> GetRootAsync(string ownerId)
        {
            return await Task.Run(() =>
                {
                    var root = Context.Directories
                            .GetQuery(d => d.Owner.Id == ownerId && d.ParentId == null)
                            .SingleOrDefault()
                            ?? throw new InvalidDataException("Can't find root folder user with this Id in database");

                    return DtoMapper.Map<DirectoryEntryDto>(root);
                }
            );
        }

        #endregion

        #region Create Methods

        /// <exception cref="InvalidDataException">
        ///     Thrown when DTO doesn't have ParentId or DirectoryEntry with this id doesn't exist in database
        /// </exception>
        public override void Add(DirectoryEntryDto directoryEntryDto)
        {
            if (!directoryEntryDto.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't find parent folder: there is no parentId");
            }

            var parentDir = Context.Directories.FindByKey(directoryEntryDto.ParentId.Value)
                            ?? throw new InvalidDataException("Can't find parent folder user with this Id in database");

            var dir = DtoMapper.Map<DirectoryEntry>(directoryEntryDto);

            dir.Modified = dir.Created = dir.Downloaded = DateTime.Now;
            dir.Owner = parentDir.Owner;
            dir.Parent = parentDir;
            dir.NodeLevel = (short) (parentDir.NodeLevel + 1);

            parentDir.Modified = DateTime.Now;

            Context.Directories.Add(dir);
            Context.SaveChanges();
        }

        /// <exception cref="InvalidDataException">
        ///     Thrown when DTO doesn't have ParentId or DirectoryEntry with this id doesn't exist in database
        /// </exception>
        public override async Task AddAsync(DirectoryEntryDto directoryEntryDto)
        {
            await Task.Run(() => Add(directoryEntryDto));
        }

        /// <exception cref="InvalidDataException">
        ///     Thrown when user with given Id doesn't exist in the Database
        /// </exception>
        public void AddRootDirToUser(string userId)
        {
            var user = Context.Users.FindByKey(userId)
                        ?? throw new InvalidDataException("Can't find user with this Id in database");

            var rootDir = new DirectoryEntry
            {
                NodeLevel = 0,
                Created = DateTime.Now,
                Downloaded = DateTime.Now,
                Modified = DateTime.Now,
                Name = "root",
                Owner = user
            };

            Context.Directories.Add(rootDir);
            Context.SaveChanges();
        }

        /// <exception cref="InvalidDataException">
        ///     Thrown when user with given Id doesn't exist in the Database
        /// </exception>
        public async Task AddRootDirToUserAsync(string userId)
        {
            await Task.Run(() => AddRootDirToUser(userId));
        }

        #endregion

        #region Update Methods

        public void Rename(DirectoryEntryDto editedDirEntryDto)
        {
            var currDirectoryEntry = Context.Directories.FindByKey(editedDirEntryDto.Id);

            if (currDirectoryEntry?.Parent == null)
            {
                throw new InvalidDataException(
                    "User folder with this Id doesn't exist or folder don't have parent");
            }

            currDirectoryEntry.Parent.Modified = DateTime.Now;
            currDirectoryEntry.Modified = DateTime.Now;
            currDirectoryEntry.Name = editedDirEntryDto.Name;

            Context.Directories.Update(currDirectoryEntry);

            Context.SaveChanges();
        }

        public async Task RenameAsync(DirectoryEntryDto editedDirEntryDto)
        {
            await Task.Run(() => Rename(editedDirEntryDto));
        }

        #endregion

        #region Delete Methods

        public async Task DeleteAsync(Guid entryId)
        {
            await DeleteHelperAsync(entryId);
            Context.SaveChanges();
        }

        public async Task DeleteWithJobAsync(Guid entryId)
        {
            //var messageInfo = new DeleteMessageInfo { OperationType = OperationType.DeleteFolder, EntryId = entryId };

            //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.Delete);
            await DeleteAsync(entryId);
        }

        #endregion

        #region Download Methods

        public async Task DownloadWithJobAsync(Guid directoryId, Guid zipId)
        {
            //var messageInfo = new DownloadMessageInfo
            //{
            //    OperationType = OperationType.DownloadFolder,
            //    EntriesIds = new List<Guid> { directoryId },
            //    ZipId = zipId
            //};

            //await Queue.AddMessageAsync(JsonConvert.SerializeObject(messageInfo), QueueJob.Download);
            await DownloadAsync(directoryId, zipId);
        }

        public async Task DownloadAsync(Guid directoryId, Guid zipId)
        {
            var currDirectoryEntry = await Context.Directories.FindByKeyAsync(directoryId);

            if (currDirectoryEntry == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            var zipName = $"{zipId}.zip";
            var tempDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var tempPath = $"{tempDir}\\{zipName}";

            using (var fs = new FileStream(tempPath, FileMode.OpenOrCreate))
            {
                using (var archive = new ZipArchive(fs, ZipArchiveMode.Create, false))
                {
                    await WriteToFolderAsync(directoryId, archive, currDirectoryEntry.Name);
                }
            }

            using (var fs = new FileStream(tempPath, FileMode.OpenOrCreate))
            {
                await Storage.UploadFileInBlocksAsync(fs, zipName, "application/zip");
            }

            File.Delete(tempPath);
        }

        public async Task<(Stream blobStream, string blobContentType)> DownloadZip(string zipName)
        {
            return await Storage.DownloadAsync(zipName, 1);
        }

        #endregion

        #region Helper Methods

        private async Task DeleteHelperAsync(Guid entryId)
        {
            var currDirectoryEntryTask = Context.Directories.FindByKeyAsync(entryId);

            var childDirsDeleteTask = Task.Run(() =>
                {
                    var childDirIds = Context.Directories.GetQuery(d => d.ParentId == entryId).Select(dir => dir.Id);

                    Parallel.ForEach(childDirIds, async dirId => { await DeleteHelperAsync(dirId); });
                }
            );

            var childFiles = Context.Files.GetQuery(f => f.ParentId == entryId);

            Parallel.ForEach(childFiles, file =>
            {
                var removeTagsTask = Context.Tags.RemoveRangeAsync(file.Tags.Where(t => t.FileEntries.Count == 1));
                var removeFileBlobsTask = Storage.DeleteRangeAsync($"{file.Id}{Path.GetExtension(file.Name)}", $"thumbnail-{file.Id}.png");

                try
                {
                    Context.Files.Remove(file);
                    Task.WaitAll(removeTagsTask, removeFileBlobsTask);
                }
                catch (AggregateException aggregate)
                {
                    if (removeFileBlobsTask.IsFaulted)
                    {
                        Context.Files.Add(file);
                    }
                }
            });

            var currDirectoryEntry = await currDirectoryEntryTask
                                     ?? throw new InvalidDataException("Can't find parent folder user with this Id in database");

            await childDirsDeleteTask;
            Context.Directories.Remove(currDirectoryEntry);
        }

        private async Task WriteToFolderAsync(Guid dirId, ZipArchive archive, string path)
        {
            var dirs = Context.Directories.GetQuery(d => d.ParentId == dirId).Select(d => new { d.Id, d.Name });

            var writeChildDirsTask = Task.Run(() =>
                {
                    Parallel.ForEach(dirs, async dir =>
                    {
                        await WriteToFolderAsync(dir.Id, archive, $"{path}/{dir.Name}");
                    });
                }
            );

            var files = Context.Files.GetQuery(f => f.ParentId == dirId).Select(f => new { f.Id, f.Name, f.Size });

            Parallel.ForEach(files, async file =>
            {
                var blobName = $"{file.Id}{Path.GetExtension(file.Name)}";

                if (await Storage.BlobExistAsync(blobName))
                {
                    var zipEntry = archive.CreateEntry($"{path}/{file.Name}");

                    using (var zipEntryStream = zipEntry.Open())
                    {
                        await Storage.DownloadAsync(zipEntryStream, blobName, file.Size);
                    }
                }
            });

            await writeChildDirsTask;
        }

        private IQueryable<DirectoryEntry> GetQuery(
            Guid? id,
            string name,
            Guid? parentId,
            DateTime? created,
            DateTime? downloaded,
            DateTime? modified,
            string ownerId
        )
        {
            var dirs = Context.Directories.GetQuery();

            if (id.HasValue)
            {
                dirs = dirs.Where(d => d.Id == id.Value);
            }
            if (name != null)
            {
                dirs = dirs.Where(d => d.Name == name);
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Where(d => d.ParentId == parentId);
            }
            if (created.HasValue)
            {
                dirs = dirs.Where(d => d.Created == created);
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Where(d => d.Downloaded == downloaded);
            }
            if (modified.HasValue)
            {
                dirs = dirs.Where(d => d.Modified == modified);
            }
            if (ownerId != null)
            {
                dirs = dirs.Where(d => d.Owner.Id == ownerId);
            }

            return dirs;
        }

        #endregion

        #endregion
    }
}