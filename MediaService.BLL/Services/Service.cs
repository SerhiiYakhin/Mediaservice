using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;

namespace MediaService.BLL.Services
{
    public abstract class Service<TDto, TId> : IService<TDto, TId> where TDto : class
    {
        protected IUnitOfWork Database { get; }

        protected virtual dynamic Repository { get; }

        protected virtual Type EntityType { get; }

        protected Service(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public TDto FindById(TId key)
        {
            //var item = Repository.FindByKey(key);
            //Type gt = typeof(item).MakeGenericType(EntityType);
            //object t = Activator.CreateInstance(gt, this, this.GetConfiguration(ThreadType));
            //Mapper.Initialize(cfg => cfg.CreateMap<EntityType, TDto>());
            //return Mapper.Map<EntityType, TDto>(Repository.FindByKey(key));
            return Repository.FindByKey(key);
        }

        public async Task<TDto> FindByIdAsync(TId key)
        {
            return await Repository.FindByKeyAsync(key);
        }


        public IEnumerable<TDto> GetData()
        {
            return Repository.GetData();
        }

        public async Task<IEnumerable<TDto>> GetDataAsync()
        {
            return await Repository.GetDataAsync();
        }


        public IEnumerable<TDto> GetData(Expression<Func<TDto, bool>> predicate)
        {
            return Repository.GetData(predicate);
        }

        public async Task<IEnumerable<TDto>> GetDataAsync(Expression<Func<TDto, bool>> predicate)
        {
            return await Repository.GetDataAsync(predicate);
        }


        public void Add(TDto item)
        {
            Repository.Add(item);
        }

        public async Task AddAsync(TDto item)
        {
            await Repository.AddAsync(item);
        }


        public void AddRange(IEnumerable<TDto> items)
        {
            Repository.AddRange(items);
        }

        public async Task AddRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.AddRangeAsync(items);
        }


        public void Update(TDto item)
        {
            Repository.Update(item);
        }

        public async Task UpdateAsync(TDto item)
        {
            await Repository.UpdateAsync(item);
        }


        public void Remove(TDto item)
        {
            Repository.Remove(item);
        }

        public async Task RemoveAsync(TDto item)
        {
            await Repository.RemoveAsync(item);
        }


        public void RemoveRange(IEnumerable<TDto> items)
        {
            Repository.RemoveRange(items);
        }

        public async Task RemoveRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.RemoveRangeAsync(items);
        }
    }
}
