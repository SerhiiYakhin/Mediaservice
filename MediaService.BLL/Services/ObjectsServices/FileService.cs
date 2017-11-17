#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, FileEntry, Guid>, IFilesService
    {
        public FileService(IUnitOfWork uow, IStorage storage) : base(uow)
        {
            Storage = storage;
            Repository = uow.Files;
        }

        private IStorage Storage { get; }

        public async Task<IEnumerable<FileEntryDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(await Context.Files.GetDataAsync(f => f.Name == name));
        }

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

        public async Task AddFilesAsync(List<FileEntryDto> filesDto, Guid folderId)
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
                fileEntry.Owner = parentDir.Owner;
                fileEntry.Parent = parentDir;
                await Context.Files.AddAsync(fileEntry);
                await Context.SaveChangesAsync();
                await Storage.UploadAsync(fileDto.FileStream, fileEntry.Id.ToString());
            }
        }

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
    }
}