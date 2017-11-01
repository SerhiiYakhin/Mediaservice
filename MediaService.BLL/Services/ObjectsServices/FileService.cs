using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : Service<FileEntryDto, Guid>, IFilesService
    {
        private readonly ObjectsCommonService<FileEntryDto, FileEntry> _commonService;

        public FileService(IUnitOfWork uow) : base(uow)
        {
            Repository = Database.Objects;
            EntityType = typeof(FileEntry);
            CollectionEntityType = typeof(IEnumerable<FileEntry>);

            _commonService = new ObjectsCommonService<FileEntryDto, FileEntry>(DtoMapper, Database.Files);

        }

        public IEnumerable<FileEntryDto> GetByName(string name) => _commonService.GetByName(name);

        public async Task<IEnumerable<FileEntryDto>> GetByNameAsync(string name) => await _commonService.GetByNameAsync(name);

        public IEnumerable<FileEntryDto> GetByParentId(Guid id) => _commonService.GetByParentId(id);

        public async Task<IEnumerable<FileEntryDto>> GetByParentIdAsync(Guid id) => await _commonService.GetByParentIdAsync(id);

        public IEnumerable<FileEntryDto> GetBy(
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

        public async Task<IEnumerable<FileEntryDto>> GetByAsync(
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
