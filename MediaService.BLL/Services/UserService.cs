using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using static MediaService.BLL.Infrastructure.MapperModule;

namespace MediaService.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public UserDto FindById(string id)
        {
            return Mapper.Map<UserDto>(Database.Users.FindByKey(id));
        }

        public async Task<UserDto> FindByIdAsync(string id)
        {
            return Mapper.Map<UserDto>(await Database.Users.FindByKeyAsync(id));
        }

        public IEnumerable<UserDto> GetData()
        {
            return Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(Database.Users.GetData());
        }

        public async Task<IEnumerable<UserDto>> GetDataAsync()
        {
            return Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(await Database.Users.GetDataAsync());
        }

        public void Add(UserDto item)
        {
            Database.Users.Add(Mapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task AddAsync(UserDto item)
        {
            await Database.Users.AddAsync(Mapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void AddRange(IEnumerable<UserDto> items)
        {
            Database.Users.AddRange(Mapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            Database.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<UserDto> items)
        {
            await Database.Users.AddRangeAsync(Mapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            await Database.SaveChangesAsync();
        }

        public void Update(UserDto item)
        {
            Database.Users.Update(Mapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task UpdateAsync(UserDto item)
        {
            await Database.Users.UpdateAsync(Mapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void Remove(UserDto item)
        {
            Database.Users.Remove(Mapper.Map<UserProfile>(item));
            Database.SaveChanges();
        }

        public async Task RemoveAsync(UserDto item)
        {
            await Database.Users.RemoveAsync(Mapper.Map<UserProfile>(item));
            await Database.SaveChangesAsync();
        }

        public void RemoveRange(IEnumerable<UserDto> items)
        {
            Database.Users.RemoveRange(Mapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            Database.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<UserDto> items)
        {
            await Database.Users.RemoveRangeAsync(Mapper.Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
            await Database.SaveChangesAsync();
        }

        public UserDto GetUserByNick(string nickName)
        {
            return Mapper.Map<UserDto>(Database.Users.GetData(u => u.Nickname.Equals(nickName)).SingleOrDefault());
        }

        public async Task<UserDto> GetUserByNickAsync(string nickName)
        {
            return Mapper.Map<UserDto>((await Database.Users.GetDataAsync(u => u.Nickname.Equals(nickName))).SingleOrDefault());
        }
    }
}
