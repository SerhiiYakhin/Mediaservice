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
        #region Properties

        private IBlobStorage Storage { get; }

        private IQueueStorage Queue { get; }

        #endregion

        #region Constructor

        public DirectoryService(IUnitOfWork uow, IBlobStorage storage, IQueueStorage queue) : base(uow)
        {
            Storage = storage;
            Queue = queue;
            Repository = uow.Directories;
        }

        #endregion

        #region Methods

        public async Task<bool> ExistAsync(string name, Guid parentId)
        {
            return await Context.Directories.AnyAsync(d => d.Name == name && d.ParentId == parentId);
        }

        #region Select Methods

        public async Task<IEnumerable<DirectoryEntryDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(
                await Context.Directories.GetDataAsync(d => d.ParentId == id));
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
            var dirs = GetQuery(id, name, parentId, created, downloaded, modified, ownerId);

            return await Task.Run(() => DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(dirs.AsParallel().ToList()));
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id or his root folder doesn't exist in the Database</exception>
        public async Task<DirectoryEntryDto> GetRootAsync(string ownerId)
        {
            var root = (await Context.Directories.GetDataAsync(d => d.Owner.Id == ownerId && d.Name == "root")).SingleOrDefault();

            if (root == null)
            {
                throw new InvalidDataException("Can't find root folder user with this Id in database");
            }

            return await Task.Run(() => DtoMapper.Map<DirectoryEntryDto>(root));
        }

        #endregion

        #region Create Methods

        public override void Add(DirectoryEntryDto directoryEntryDto)
        {
            if (!directoryEntryDto.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't find parent folder: there is no parentId");
            }

            var parentDir = Context.Directories.FindByKey(directoryEntryDto.ParentId.Value);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;
            var dir = DtoMapper.Map<DirectoryEntry>(directoryEntryDto);
            dir.Owner = parentDir.Owner;
            dir.Parent = parentDir;
            dir.NodeLevel = (short)(parentDir.NodeLevel + 1);
            dir.Modified = dir.Created = dir.Downloaded = DateTime.Now;

            Context.Directories.Add(dir);
            Context.SaveChanges();
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id or his root folder doesn't exist in the Database</exception>
        public override async Task AddAsync(DirectoryEntryDto directoryEntryDto)
        {
            if (!directoryEntryDto.ParentId.HasValue)
            {
                throw new InvalidDataException("Can't find parent folder: there is no parentId");
            }

            var parentDir = await Context.Directories.FindByKeyAsync(directoryEntryDto.ParentId.Value);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;
            var dir = DtoMapper.Map<DirectoryEntry>(directoryEntryDto);
            dir.Owner = parentDir.Owner;
            dir.Parent = parentDir;
            dir.NodeLevel = (short)(parentDir.NodeLevel + 1);
            dir.Modified = dir.Created = dir.Downloaded = DateTime.Now;

            await Context.Directories.AddAsync(dir);
            await Context.SaveChangesAsync();
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id doesn't exist in the Database</exception>
        public async Task AddRootDirToUserAsync(string userId)
        {
            var rootDir = new DirectoryEntry
            {
                NodeLevel = 0,
                Created = DateTime.Now,
                Downloaded = DateTime.Now,
                Modified = DateTime.Now,
                Name = "root"
            };
            var user = await Context.Users.FindByKeyAsync(userId);
            rootDir.Owner = user ?? throw new InvalidDataException("Can't find user with this Id in database");

            await Context.Directories.AddAsync(rootDir);
            await Context.SaveChangesAsync();
        }

        #endregion

        #region Update Methods

        public async Task RenameAsync(DirectoryEntryDto editedDirEntryDto)
        {
            var currDirectoryEntry = await Context.Directories.FindByKeyAsync(editedDirEntryDto.Id);

            if (currDirectoryEntry == null)
            {
                throw new InvalidDataException("Can't find user's folder with this Id in database");
            }

            if (currDirectoryEntry.Parent != null)
            {
                currDirectoryEntry.Parent.Modified = DateTime.Now;
            }

            currDirectoryEntry.Modified = DateTime.Now;
            currDirectoryEntry.Name = editedDirEntryDto.Name;

            await Context.Directories.UpdateAsync(currDirectoryEntry);
            await Context.SaveChangesAsync();
        }

        #endregion

        #region Delete Methods

        public async Task DeleteAsync(Guid entryId)
        {
            var currDirectoryEntry = await Context.Directories.FindByKeyAsync(entryId);

            if (currDirectoryEntry == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            var childFiles = await Context.Files.GetDataAsync(f => f.ParentId == entryId);
            var childDirs = await Context.Directories.GetDataAsync(f => f.ParentId == entryId);


            foreach (var file in childFiles)
            {
                await Context.Files.RemoveAsync(file);
                await Storage.DeleteAsync($"{file.Id}{Path.GetExtension(file.Name)}");
            }

            foreach (var dir in childDirs)
            {
                await DeleteAsync(dir.Id);
            }

            await Context.Directories.RemoveAsync(currDirectoryEntry);

            await Context.SaveChangesAsync();
        }

        public async Task DeleteWithJobAsync(Guid entryId)
        {
            await DeleteAsync(entryId);
        }

        #endregion

        #region Download Methods

        public async Task DownloadWithJobAsync(Guid directoryId, Guid zipId)
        {
            await DownloadAsync(directoryId, zipId);
            //await Queue.AddMessageAsync(directoryId.ToString(), QueueJob.DataOperation);
        }

        public async Task DownloadAsync(Guid directoryId, Guid zipId)
        {
            var currDirectoryEntry = await Context.Directories.FindByKeyAsync(directoryId);

            if (currDirectoryEntry == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            var zipName = $"{zipId}.zip";

            using (var fs = new FileStream(zipName, FileMode.OpenOrCreate))
            {
                using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create, false))
                {
                    await WriteToFolderAsync(directoryId, archive, currDirectoryEntry.Name);
                }

                fs.Position = 0;
                await Storage.UploadFileInBlocksAsync(fs, zipName, "application/zip");
            }

            File.Delete(zipName);
        }

        public async Task<(Stream blobStream, bool blobExist)> DownloadZip(string zipName)
        {
            return await Storage.DownloadAsync(zipName);
        }

        #endregion

        #region Helper Methods

        private async Task WriteToFolderAsync(Guid dirId, ZipArchive archive, string path)
        {
            var files = await Context.Files.GetDataAsync(f => f.ParentId == dirId);
            var dirs = await Context.Files.GetDataAsync(f => f.ParentId == dirId);

            foreach (var file in files)
            {
                var blobName = $"{file.Id}{Path.GetExtension(file.Name)}";

                if (await Storage.BlobExistAsync(blobName))
                {
                    var zipEntry = archive.CreateEntry($"{path}/{file.Name}");

                    using (var zipEntryStream = zipEntry.Open())
                    {
                        await Storage.DownloadAsync(blobName, zipEntryStream);
                    }
                }
            }

            foreach (var dir in dirs)
            {
                await WriteToFolderAsync(dir.Id, archive, $"{path}/{dir.Name}");
            }
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
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Id == id.Value));
            }
            if (name != null)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Name == name));
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.ParentId == parentId));
            }
            if (created.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Created == created));
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Downloaded == downloaded));
            }
            if (modified.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Modified == modified));
            }
            if (ownerId != null)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Owner.Id == ownerId));
            }

            return dirs;
        }

        #endregion

        #endregion
    }
}