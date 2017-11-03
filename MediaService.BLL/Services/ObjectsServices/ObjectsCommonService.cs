using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public sealed class ObjectsCommonService<TObjectDto, TObject>
        where TObjectDto : ObjectEntryDto
        where TObject : ObjectEntry
    {
        private IMapper DtoMapper { get; }

        private static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        private IRepository<TObject, Guid> Repository { get; }

        public ObjectsCommonService(IMapper dtoMapper, IRepository<TObject, Guid> repository)
        {
            DtoMapper = dtoMapper;
            Repository = repository;
        }

        public IEnumerable<TObjectDto> GetByName(string name)
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.Name.Equals(name)));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
        }

        public async Task<IEnumerable<TObjectDto>> GetByNameAsync(string name)
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.Name.Equals(name)));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
        }

        public IEnumerable<TObjectDto> GetByParentId(Guid id)
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TObjectDto>>(Repository.GetDataParallel(o => o.ParentId.Equals(id)));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
        }

        public async Task<IEnumerable<TObjectDto>> GetByParentIdAsync(Guid id)
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TObjectDto>>(await Repository.GetDataAsyncParallel(o => o.ParentId.Equals(id)));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
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
            try
            {
                var objects = GetQuery(name, parentId, size, created, downloaded, modified, owners);
                return DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
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
            try
            {
                var objects = GetQuery(name, parentId, size, created, downloaded, modified, owners);
                return await Task.Run(() => DtoMapper.Map<IEnumerable<TObjectDto>>(objects.AsParallel()));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TObjectDto>();
            }
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
