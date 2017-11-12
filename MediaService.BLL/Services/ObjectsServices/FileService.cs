using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, FileEntry, Guid>, IFilesService
    {
        public FileService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Files;
        }

        public async Task<IEnumerable<FileEntryDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(await Repository.GetDataAsyncParallel(f => f.Name.Equals(name)));
        }

        public async Task<IEnumerable<FileEntryDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<FileEntryDto>>(await Repository.GetDataAsyncParallel(f => f.ParentId.Equals(id)));
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
            var dirs = Repository.GetQuery();
            if (id.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Id.Equals(id.Value)));
            }
            if (name != null)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Name.Equals(name)));
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.ParentId.Equals(parentId)));
            }
            if (created.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Created.Equals(created)));
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Downloaded.Equals(downloaded)));
            }
            if (modified.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Modified.Equals(modified)));
            }
            if (ownerId != null)
            {
                dirs = dirs.Intersect(Repository.GetQuery(f => f.Owner.Id.Equals(ownerId)));
            }

            return dirs;
        }
    }
}
