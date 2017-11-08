using MediaService.BLL.DTO;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class ObjectsCommonService<TObjectDto, TObject> : Service<TObjectDto, TObject, Guid>
        where TObjectDto : ObjectEntryDto
        where TObject : ObjectEntry
    {

        public ObjectsCommonService(IUnitOfWork uow) : base(uow) { }

        public IEnumerable<TObjectDto> GetByName(string name)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.Name.Equals(name)));
        }

        public async Task<IEnumerable<TObjectDto>> GetByNameAsync(string name)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.Name.Equals(name)));
        }

        public IEnumerable<TObjectDto> GetByParentId(Guid id)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.ParentId.Equals(id)));
        }

        public async Task<IEnumerable<TObjectDto>> GetByParentIdAsync(Guid id)
        {
            return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.ParentId.Equals(id)));
        }

        public IEnumerable<TObjectDto> GetBy(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
            )
        {
            var objects = GetQuery(name, parentId, size, created, downloaded, modified, owners);
            return DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel());
        }

        public async Task<IEnumerable<TObjectDto>> GetByAsync(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
        )
        {
            var objects = GetQuery(name, parentId, size, created, downloaded, modified, owners);
            return await Task.Run(() => DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel()));
        }

        private IQueryable<TObject> GetQuery(string name, Guid? parentId, long? size, DateTime? created, DateTime? downloaded, DateTime? modified, ICollection<UserDto> owners)
        {
            var objects = Repository.GetQuery();
            if (name != null)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Name.Equals(name)));
            }
            if (parentId.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.ParentId.Equals(parentId)));
            }
            if (size.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Size.Equals(size)));
            }
            if (created.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Created.Equals(created)));
            }
            if (downloaded.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Downloaded.Equals(downloaded)));
            }
            if (modified.HasValue)
            {
                objects = objects.Intersect(Repository.GetQuery(o => o.Modified.Equals(modified)));
            }
            if (owners != null)
            {
                var ownersCollection = DtoMapper.Map<ICollection<UserProfile>>(owners);
                objects = objects.Intersect(Repository.GetQuery(o => o.Owners.Intersect(ownersCollection).Any()));

            }

            return objects;
        }
    }
}
