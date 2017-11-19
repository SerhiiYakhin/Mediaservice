#region usings

using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.BLL.Services
{
    public abstract class Service<TDto, TEntity, TId> : IService<TDto, TId>
        where TDto : class
        where TEntity : class
    {
        #region Fields

        private IMapper _mapper;

        #endregion

        #region Properties

        protected IUnitOfWork Context { get; }

        protected IRepository<TEntity, TId> Repository { get; set; }

        protected IMapper DtoMapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        #endregion

        #region Constructor

        protected Service(IUnitOfWork uow)
        {
            Context = uow;
        }

        #endregion

        #region Methods

        #region Select Methods

        public virtual TDto FindById(TId key)
        {
            return DtoMapper.Map<TDto>(Repository.FindByKey(key));
        }

        public virtual async Task<TDto> FindByIdAsync(TId key)
        {
            return DtoMapper.Map<TDto>(await Repository.FindByKeyAsync(key));
        }

        public virtual IEnumerable<TDto> GetData()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetData());
        }

        public virtual async Task<IEnumerable<TDto>> GetDataAsync()
        {
            return DtoMapper.Map<IEnumerable<TDto>>(await Repository.GetDataAsync());
        }

        #endregion

        #region Delete Methods

        //public virtual void Remove(TDto item)
        //{
        //    Repository.Remove(DtoMapper.Map<TEntity>(item));
        //    Context.SaveChanges();
        //}

        //public virtual async Task RemoveAsync(TDto item)
        //{
        //    await Repository.RemoveAsync(DtoMapper.Map<TEntity>(item));
        //    await Context.SaveChangesAsync();
        //}

        //public virtual void RemoveRange(IEnumerable<TDto> items)
        //{
        //    Repository.RemoveRange(DtoMapper.Map<IEnumerable<TEntity>>(items));
        //    Context.SaveChanges();
        //}

        //public virtual async Task RemoveRangeAsync(IEnumerable<TDto> items)
        //{
        //    await Repository.RemoveRangeAsync(DtoMapper.Map<IEnumerable<TEntity>>(items));
        //    await Context.SaveChangesAsync();
        //}

        #endregion

        #region Abstract Methods

        public abstract void Add(TDto item);

        public abstract Task AddAsync(TDto item);

        //public abstract void AddRange(IEnumerable<TDto> items);

        //public abstract Task AddRangeAsync(IEnumerable<TDto> items);

        //public abstract void Update(TDto item);

        //public abstract Task UpdateAsync(TDto item);

        #endregion

        public void Dispose()
        {
            Context.Dispose();
        }

        #endregion
    }
}