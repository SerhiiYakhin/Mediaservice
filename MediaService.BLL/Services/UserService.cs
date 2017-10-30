using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow) => Database = uow;
        
        public void Dispose() => Database.Dispose();

        public UserDto GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public bool UserExist(Func<UserDto, bool> predicate)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Func<UserDto, bool>, Func<UserProfile, bool>>());
            var userPredicate = Mapper.Map<Func<UserDto, bool>, Func<UserProfile, bool>>(predicate);

            return Database.Users.Get(userPredicate).Any();
        }

        public IEnumerable<UserDto> GetUsers(Predicate<UserDto> predicate)
        {
            throw new NotImplementedException();
        }

        public OperationDetails EditUser(UserDto item)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetUsersAsync(Predicate<UserDto> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> EditUserAsync(UserDto item)
        {
            throw new NotImplementedException();
        }

        public Task CreateUserAsync(UserDto userDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDto, UserProfile>());
            var user = Mapper.Map<UserDto, UserProfile>(userDto);

            Database.Users.Create(user);
            return Database.SaveAsync();
        }

        public Task DeleteUserAsync(UserDto userDto)
        {
            throw new NotImplementedException();
        }
    }
}
