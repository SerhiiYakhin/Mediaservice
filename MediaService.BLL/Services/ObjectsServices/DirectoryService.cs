using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class DirectoryService : Service<DirectoryEntryDto, DirectoryEntry, Guid>, IDirectoryService
    {
        public DirectoryService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Directories;
        }
        
        public async Task<IEnumerable<DirectoryEntryDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(await Repository.GetDataAsyncParallel(d => d.Name.Equals(name)));
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(await Repository.GetDataAsyncParallel(d => d.ParentId.Equals(id)));
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
            var dirs = Repository.GetQuery();
            if (id.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Id.Equals(id.Value)));
            }
            if (name != null)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Name.Equals(name)));
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.ParentId.Equals(parentId)));
            }
            if (created.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Created.Equals(created)));
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Downloaded.Equals(downloaded)));
            }
            if (modified.HasValue)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Modified.Equals(modified)));
            }
            if (ownerId != null)
            {
                dirs = dirs.Intersect(Repository.GetQuery(d => d.Owner.Id == ownerId));
            }

            return dirs;
        }

        public async Task<DirectoryEntryDto> GetRootAsync(string ownerId)
        {
            return await Task.Run(async () => DtoMapper.Map<DirectoryEntryDto>(await Repository.GetDataAsyncParallel(d => d.Owner.Id.Equals(ownerId))));
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
                Thumbnail = "~/fonts/icons-buttons/folder.svg",
                Name = "root"
            };
            var user = await Database.Users.FindByKeyAsync(userId);
            rootDir.Owner = user ?? throw new InvalidDataException("Can't find user with this Id in database");

            await Repository.AddAsync(rootDir);
            await Database.SaveChangesAsync();
        }
    }
}
