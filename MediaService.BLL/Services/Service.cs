using AutoMapper;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;
using Microsoft.AspNet.Identity;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
    public abstract class Service<TDto, TId> : IService<TDto, TId> where TDto : class
    {
        private IMapper _mapper;

        //todo: check if it set right by inheritance
        protected static Logger Logger { get; } = LogManager.GetCurrentClassLogger();

        protected IUnitOfWork Database { get; }

        protected dynamic Repository { get; set; }

        protected Type EntityType { get; set; }

        protected Type CollectionEntityType { get; set; }

        protected IMapper DtoMapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        protected Service(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();


        public TDto FindById(TId key)
        {
            try
            {
                return DtoMapper.Map<TDto>(Repository.FindByKey(key));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public async Task<TDto> FindByIdAsync(TId key)
        {
            try
            {
                return await DtoMapper.Map<TDto>(Repository.FindByKeyAsync(key));
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }


        public IEnumerable<TDto> GetData()
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetData());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TDto>();
            }
        }

        public async Task<IEnumerable<TDto>> GetDataAsync()
        {
            try
            {
                return await DtoMapper.Map<IEnumerable<TDto>>(Repository.GetDataAsync());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TDto>();
            }
        }


        public IEnumerable<TDto> GetDataParallel()
        {
            try
            {
                return DtoMapper.Map<IEnumerable<TDto>>(Repository.GetDataParallel());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TDto>();
            }
        }

        public async Task<IEnumerable<TDto>> GetDataAsyncParallel()
        {
            try
            {
                return await DtoMapper.Map<IEnumerable<TDto>>(Repository.GetDataAsyncParallel());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return Enumerable.Empty<TDto>();
            }
        }


        public IdentityResult Add(TDto item)
        {
            try
            {
                Repository.Add(DtoMapper.Map(item, typeof(TDto), EntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> AddAsync(TDto item)
        {
            try
            {
                await Repository.AddAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult AddRange(IEnumerable<TDto> items)
        {
            try
            {
                Repository.AddRange(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> AddRangeAsync(IEnumerable<TDto> items)
        {
            try
            {
                await Repository.AddRangeAsync(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult AddRangeParallel(IEnumerable<TDto> items)
        {
            try
            {
                Repository.AddRangeParallel(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> AddRangeAsyncParallel(IEnumerable<TDto> items)
        {
            try
            {
                await Repository.AddRangeAsyncParallel(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult Update(TDto item)
        {
            try
            {
                Repository.Update(DtoMapper.Map(item, typeof(TDto), EntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> UpdateAsync(TDto item)
        {
            try
            {
                await Repository.UpdateAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult Remove(TDto item)
        {
            try
            {
                Repository.Remove(DtoMapper.Map(item, typeof(TDto), EntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> RemoveAsync(TDto item)
        {
            try
            {
                await Repository.RemoveAsync(DtoMapper.Map(item, typeof(TDto), EntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult RemoveRange(IEnumerable<TDto> items)
        {
            try
            {
                Repository.RemoveRange(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> RemoveRangeAsync(IEnumerable<TDto> items)
        {
            try
            {
                await Repository.RemoveRangeAsync(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }


        public IdentityResult RemoveRangeParallel(IEnumerable<TDto> items)
        {
            try
            {
                Repository.RemoveRangeParallel(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                Database.SaveChanges();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }

        public async Task<IdentityResult> RemoveRangeAsyncParallel(IEnumerable<TDto> items)
        {
            try
            {
                await Repository.RemoveRangeAsyncParallel(DtoMapper.Map(items, typeof(IEnumerable<TDto>), CollectionEntityType));
                await Database.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return new IdentityResult(e.Message);
            }
        }
    }
}
