#define ABSTRACT_CLASS_INHERITANCE

using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
#if ABSTRACT_CLASS_INHERITANCE

    public sealed class UserService : Service<UserDto, string>, IUserService
    {
        public UserService(IUnitOfWork uow) : base(uow)
        {
            Repository = Database.Users;

            EntityType = typeof(UserProfile);
            
            CollectionEntityType = typeof(IEnumerable<UserProfile>);
        }

        public UserDto GetUserByNick(string nickName)
        {
            try
            {
                return DtoMapper.Map<UserDto>(Database.Users.GetDataParallel(u => u.Nickname.Equals(nickName)).SingleOrDefault());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public async Task<UserDto> GetUserByNickAsync(string nickName)
        {
            try
            {
                return DtoMapper.Map<UserDto>((await Database.Users.GetDataAsyncParallel(u => u.Nickname.Equals(nickName))).SingleOrDefault());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }
    }

#else    

    public sealed class UserService : IUserService
    {
        private IMapper _mapper;

        private IUnitOfWork Database { get; }

        private IMapper DtoMapper => _mapper ?? (_mapper = MapperModule.GetMapper());

        public UserService(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public UserDto FindById(string id)
        {
            return DtoMapper.Map<UserDto>(Database.Users.FindByKey(id));
        }

        public async Task<UserDto> FindByIdAsync(string id)
        {
            return DtoMapper.Map<UserDto>(await Database.Users.FindByKeyAsync(id));
        }

        public IEnumerable<UserDto> GetData()
        {
            return DtoMapper.Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(Database.Users.GetData());
        }

        public async Task<IEnumerable<UserDto>> GetDataAsync()
        {
            return DtoMapper.Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(await Database.Users.GetDataAsync());
        }

        public void Add(UserDto item)
        {
            Database.Users.Add(DtoMapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task AddAsync(UserDto item)
        {
            await Database.Users.AddAsync(DtoMapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<UserDto> items)
        {
            Database.Users.AddRange(DtoMapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            Database.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<UserDto> items)
        {
            await Database.Users.AddRangeAsync(DtoMapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            await Database.SaveChangesAsync();
        }

        public void Update(UserDto item)
        {
            Database.Users.Update(DtoMapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task UpdateAsync(UserDto item)
        {
            await Database.Users.UpdateAsync(DtoMapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void Remove(UserDto item)
        {
            Database.Users.Remove(DtoMapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task RemoveAsync(UserDto item)
        {
            await Database.Users.RemoveAsync(Mapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<UserDto> items)
        {
            Database.Users.RemoveRange(DtoMapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            Database.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<UserDto> items)
        {
            await Database.Users.RemoveRangeAsync(DtoMapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            await Database.SaveChangesAsync();
        }

        public UserDto GetUserByNick(string nickName)
        {
            return DtoMapper.Map<UserDto>(Database.Users.GetData(u => u.Nickname.Equals(nickName)).SingleOrDefault());
        }

        public async Task<UserDto> GetUserByNickAsync(string nickName)
        {
            return DtoMapper.Map<UserDto>((await Database.Users.GetDataAsync(u => u.Nickname.Equals(nickName))).SingleOrDefault());
        }
    }

#endif
}
