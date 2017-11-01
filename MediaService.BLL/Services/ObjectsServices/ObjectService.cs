using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class ObjectService : Service<ObjectEntryDto, Guid>, IObjectService<ObjectEntryDto>
    {
        private readonly ObjectsCommonService<ObjectEntryDto, ObjectEntry> _commonService;

        public ObjectService(IUnitOfWork uow) : base(uow)
        {
            Repository = Database.Objects;
            EntityType = typeof(ObjectEntry);
            CollectionEntityType = typeof(IEnumerable<ObjectEntry>);

            _commonService = new ObjectsCommonService<ObjectEntryDto, ObjectEntry>(DtoMapper, Database.Objects);
        }

        public IEnumerable<ObjectEntryDto> GetByName(string name) => _commonService.GetByName(name);

        public async Task<IEnumerable<ObjectEntryDto>> GetByNameAsync(string name) => await _commonService.GetByNameAsync(name);

        public IEnumerable<ObjectEntryDto> GetByParentId(Guid id) => _commonService.GetByParentId(id);

        public async Task<IEnumerable<ObjectEntryDto>> GetByParentIdAsync(Guid id) => await _commonService.GetByParentIdAsync(id);

        public IEnumerable<ObjectEntryDto> GetBy(
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

        public async Task<IEnumerable<ObjectEntryDto>> GetByAsync(
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
