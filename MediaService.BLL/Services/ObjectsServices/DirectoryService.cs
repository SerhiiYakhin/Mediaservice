using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class DirectoryService : Service<DirectoryEntryDto, Guid>, IDirectoryService
    {
        private readonly ObjectsCommonService<DirectoryEntryDto, DirectoryEntry> _commonService;

        public DirectoryService(IUnitOfWork uow) : base(uow)
        {
            Repository = Database.Objects;
            EntityType = typeof(DirectoryEntry);
            _commonService = new ObjectsCommonService<DirectoryEntryDto, DirectoryEntry>(DtoMapper, Database.Directories);

            CollectionEntityType = typeof(IEnumerable<DirectoryEntry>);
        }

        public IEnumerable<DirectoryEntryDto> GetByName(string name) => _commonService.GetByName(name);

        public async Task<IEnumerable<DirectoryEntryDto>> GetByNameAsync(string name) => await _commonService.GetByNameAsync(name);

        public IEnumerable<DirectoryEntryDto> GetByParentId(Guid id) => _commonService.GetByParentId(id);

        public async Task<IEnumerable<DirectoryEntryDto>> GetByParentIdAsync(Guid id) => await _commonService.GetByParentIdAsync(id);

        public IEnumerable<DirectoryEntryDto> GetBy(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
        )
        {
            return _commonService.GetBy(name, parentId, size, created, downloaded, modified, owners);
        }

        public async Task<IEnumerable<DirectoryEntryDto>> GetByAsync(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
        )
        {
            return await _commonService.GetByAsync(name, parentId, size, created, downloaded, modified, owners);
        }
    }
}
