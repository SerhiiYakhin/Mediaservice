using AutoMapper;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
    public abstract class Service<TDto, TEntity, TId> : IService<TDto, TId>
        where TDto : class
        where TEntity : class
    {
        private IMapper _mapper;

        protected IUnitOfWork Database { get; }

        protected IRepository<TEntity, TId> Repository { get; set; }

        protected IMapper DtoMapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        protected Service(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();


        public TDto FindById(TId key)
        {
            return DtoMapper.Map<TDto>(Repository.FindByKey(key));
        }

        public async Task<TDto> FindByIdAsync(TId key)
        {
            return DtoMapper.Map<TDto>(await Repository.FindByKeyAsync(key));
        }


        public IEnumerable<TDto> GetData()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetData());
        }

        public async Task<IEnumerable<TDto>> GetDataAsync()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(await Repository.GetDataAsync());
        }


        public IEnumerable<TDto> GetDataParallel()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetDataParallel());
        }

        public async Task<IEnumerable<TDto>> GetDataAsyncParallel()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(await Repository.GetDataAsyncParallel());
        }

        public void Add(TDto item)
        {
            Repository.Add(DtoMapper.Map<TEntity>(item));
            Database.SaveChanges();
        }

        public async Task AddAsync(TDto item)
        {
            await Repository.AddAsync(DtoMapper.Map<TEntity>(item));
            await Database.SaveChangesAsync();
        }


        public void AddRange(IEnumerable<TDto> items)
        {
            Repository.AddRange(DtoMapper.Map< IEnumerable<TEntity>>(items));
            Database.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.AddRangeAsync(DtoMapper.Map<IEnumerable<TEntity>>(items));
            await Database.SaveChangesAsync();
        }


        public void AddRangeParallel(IEnumerable<TDto> items)
        {
            Repository.AddRangeParallel(DtoMapper.Map<IEnumerable<TEntity>>(items));
            Database.SaveChanges();
        }

        public async Task AddRangeAsyncParallel(IEnumerable<TDto> items)
        {
            await Repository.AddRangeAsyncParallel(DtoMapper.Map<IEnumerable<TEntity>>(items));
            await Database.SaveChangesAsync();
        }


        public void Update(TDto item)
        {
            Repository.Update(DtoMapper.Map<TEntity>(item));
            Database.SaveChanges();
        }

        public async Task UpdateAsync(TDto item)
        {
            await Repository.UpdateAsync(DtoMapper.Map<TEntity>(item));
            await Database.SaveChangesAsync();
        }


        public void Remove(TDto item)
        {
            Repository.Remove(DtoMapper.Map<TEntity>(item));
            Database.SaveChanges();
        }

        public async Task RemoveAsync(TDto item)
        {
            await Repository.RemoveAsync(DtoMapper.Map<TEntity>(item));
            await Database.SaveChangesAsync();
        }


        public void RemoveRange(IEnumerable<TDto> items)
        {
            Repository.RemoveRange(DtoMapper.Map<IEnumerable<TEntity>>(items));
            Database.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<TDto> items)
        {
            await Repository.RemoveRangeAsync(DtoMapper.Map<IEnumerable<TEntity>>(items));
            await Database.SaveChangesAsync();
        }


        public void RemoveRangeParallel(IEnumerable<TDto> items)
        {
            Repository.RemoveRangeParallel(DtoMapper.Map<IEnumerable<TEntity>>(items));
            Database.SaveChanges();
        }

        public async Task RemoveRangeAsyncParallel(IEnumerable<TDto> items)
        {
            await Repository.RemoveRangeAsyncParallel(DtoMapper.Map<IEnumerable<TEntity>>(items));
            await Database.SaveChangesAsync();
        }
    }
}
