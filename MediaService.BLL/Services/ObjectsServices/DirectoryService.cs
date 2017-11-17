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
    public class DirectoryService : Service<DirectoryEntryDto, DirectoryEntry, Guid>, IDirectoryService
    {
        public DirectoryService(IUnitOfWork uow) : base(uow)
        {
            Repository = Context.Directories;
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<DirectoryEntryDto>>(
                await Context.Directories.GetDataAsync(d => d.Name == name));
        }

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
        public override async Task AddAsync(DirectoryEntryDto directoryEntryDto)
        {
            var parentDir = await Context.Directories.FindByKeyAsync(directoryEntryDto.ParentId.Value);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;
            var dir = DtoMapper.Map<DirectoryEntry>(directoryEntryDto);
            dir.Owner = parentDir.Owner;
            dir.Parent = parentDir;
            dir.NodeLevel = (short) (parentDir.NodeLevel + 1);

            await Repository.AddAsync(dir);
            await Context.SaveChangesAsync();
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id or his root folder doesn't exist in the Database</exception>
        public async Task AddAsync(string name, Guid parentId)
        {
            var parentDir = await Context.Directories.FindByKeyAsync(parentId);

            if (parentDir == null)
            {
                throw new InvalidDataException("Can't find parent folder user with this Id in database");
            }

            parentDir.Modified = DateTime.Now;

            var dir = new DirectoryEntry
            {
                Name = name,
                Created = DateTime.Now,
                Downloaded = DateTime.Now,
                Modified = DateTime.Now,
                NodeLevel = (short) (parentDir.NodeLevel + 1),
                Parent = parentDir,
                Thumbnail = parentDir.Thumbnail,
                Owner = parentDir.Owner,
                Viewers = parentDir.Viewers
            };

            await Repository.AddAsync(dir);
            await Context.SaveChangesAsync();
        }

        /// <exception cref="InvalidDataException">Thrown when user with given Id or his root folder doesn't exist in the Database</exception>
        public async Task<DirectoryEntryDto> GetRootAsync(string ownerId)
        {
            var root = (await Context.Directories.GetDataAsync(d => d.Owner.Id == ownerId)).SingleOrDefault();

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
    }
}