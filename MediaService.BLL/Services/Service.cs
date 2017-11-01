using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediaService.BLL.Infrastructure;

namespace MediaService.BLL.Services
{
    public abstract class Service<TDto, TId> : IService<TDto, TId> where TDto : class
    {
        private IMapper _mapper;

        protected IUnitOfWork Database { get; }

        protected dynamic Repository { get; set; }

        protected Type EntityType { get; set; }

        protected Type CollectionEntityType { get; set; }

        protected IMapper DtoMapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        protected Service(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public TDto FindById(TId key)
        {
            return DtoMapper.Map<TDto>(Repository.FindByKey(key));
        }

        public async Task<TDto> FindByIdAsync(TId key)
        {
            return await DtoMapper.Map<TDto>(Repository.FindByKeyAsync(key));
        }


        public IEnumerable<TDto> GetData()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetData());
        }

        public async Task<IEnumerable<TDto>> GetDataAsync()
        {
            return await DtoMapper.Map<IEnumerable<TDto>>(Repository.GetDataAsync());
        }


        public void Add(TDto item)
        {
            Repository.Add(DtoMapper.Map(item, typeof(TDto), EntityType));
            Database.SaveChanges();
        }

        public async Task AddAsync(TDto item)
        {
            await Repository.AddAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
            await Database.SaveChangesAsync();
        }


        public void AddRange(IEnumerable<TDto> items)
        {
            Repository.AddRange(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
            Database.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.AddRangeAsync(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
            await Database.SaveChangesAsync();
        }


        public void Update(TDto item)
        {
            Repository.Update(DtoMapper.Map(item, typeof(TDto), EntityType));
            Database.SaveChanges();
        }

        public async Task UpdateAsync(TDto item)
        {
            await Repository.UpdateAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
            await Database.SaveChangesAsync();
        }


        public void Remove(TDto item)
        {
            Repository.Remove(DtoMapper.Map(item, typeof(TDto), EntityType));
            Database.SaveChanges();
        }

        public async Task RemoveAsync(TDto item)
        {
            await Repository.RemoveAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
            await Database.SaveChangesAsync();
        }


        public void RemoveRange(IEnumerable<TDto> items)
        {
            Repository.RemoveRange(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
            Database.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.RemoveRangeAsync(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
            await Database.SaveChangesAsync();
        }
    }
}
