#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;
using System.Web;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, FileEntry, Guid>, IFilesService
    {
        #region Fields



        #endregion

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

            foreach (var fileDto in filesDto)
            {
                var fileEntry = DtoMapper.Map<FileEntry>(fileDto);
                var mimeType = MimeMapping.GetMimeMapping(fileEntry.Name);

                fileEntry.Owner = parentDir.Owner;
                fileEntry.Parent = parentDir;
                await Context.Files.AddAsync(fileEntry);
                await Context.SaveChangesAsync();
                await Storage.UploadAsync(fileDto.FileStream, $"{fileEntry.Id}{Path.GetExtension(fileEntry.Name)}", mimeType);
            }
        }

        public async Task RenameAsync(FileEntryDto editedDirEntryDto)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid entryId)
        {
            throw new NotImplementedException();
        }

        public async Task DownloadWithJobAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            throw new NotImplementedException();
        }

        public async Task DownloadAsync(IEnumerable<Guid> filesIds, Guid zipId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetLinkToFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetLinkToFileAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<Stream> GetFileThumbnailAsync(Guid fileId)
        {
            throw new NotImplementedException();
        }

        public async Task AddTagAsync(Guid fileId, string tagName)
        {
            throw new NotImplementedException();
        }

        public override void Add(FileEntryDto item)
        {
            throw new NotImplementedException();
        }

        public override Task AddAsync(FileEntryDto item)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Update Methods


        #endregion

        #region Delete Methods



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

        #endregion

        #endregion
    }
}