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
            Repository = Context.Directories;
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(await Context.Directories.GetDataAsyncParallel(d => d.Name.Equals(name)));
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(await Context.Directories.GetDataAsyncParallel(d => d.ParentId.Equals(id)));
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
            var dirs = Context.Directories.GetQuery();
            if (id.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Id.Equals(id.Value)));
            }
            if (name != null)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Name.Equals(name)));
            }
            if (parentId.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.ParentId.Equals(parentId)));
            }
            if (created.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Created.Equals(created)));
            }
            if (downloaded.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Downloaded.Equals(downloaded)));
            }
            if (modified.HasValue)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Modified.Equals(modified)));
            }
            if (ownerId != null)
            {
                dirs = dirs.Intersect(Context.Directories.GetQuery(d => d.Owner.Id == ownerId));
            }

            return dirs;
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id or his root folder doesn't exist in the Database</exception>
        public async Task<DirectoryEntryDto> GetRootAsync(string ownerId)
        {
            var x = await Context.Directories.GetDataAsync(d => d.Owner.Id.Equals(ownerId));
            var root = x.SingleOrDefault();
            //var x2 = Context.Directories.GetDataParallel(d => d.Owner.Id.Equals(ownerId));
            //var root2 = x2.SingleOrDefault();
            //var root = (await Context.Directories.GetDataAsyncParallel(d => d.Owner.Id.Equals(ownerId))).SingleOrDefault();

            if (root == null)
            {
                throw new InvalidDataException("Can't find root folder user with this Id in database");
            }

            return await Task.Run(() => DtoMapper.Map<DirectoryEntryDto>(root));
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
            var user = await Context.Users.FindByKeyAsync(userId);
            rootDir.Owner = user ?? throw new InvalidDataException("Can't find user with this Id in database");

            await Context.Directories.AddAsync(rootDir);
            await Context.SaveChangesAsync();
        }
    }
}
