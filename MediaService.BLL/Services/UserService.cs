using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow) => Database = uow;
        
        public void Dispose() => Database.Dispose();

        public bool UserExist(Expression<Func<UserDto, bool>> predicate)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Expression<Func<UserDto, bool>>, Expression<Func<UserProfile, bool>>>());
            var userPredicate = Mapper.Map<Expression<Func<UserDto, bool>>, Expression<Func<UserProfile, bool>>>(predicate);

            return Database.Users.Get(userPredicate).Any();
        }

        public UserDto GetUserById(string id)
        {
            throw new NotImplementedException();
        }

        public UserDto GetUserByNick(string nickName)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserProfile, UserDto>());

            var user = Database.Users.Get(u => u.Nickname.Equals(nickName)).SingleOrDefault();

            return Mapper.Map<UserProfile, UserDto>(user);
        }

        public IEnumerable<UserDto> GetUsers(Expression<Func<UserDto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public OperationDetails EditUser(UserDto item)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserDto userDto)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDto, UserProfile>());

            var user = Mapper.Map<UserDto, UserProfile>(userDto);

            Database.Users.Create(user);
            Database.SaveAsync();
        }

        public void DeleteUser(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByNickAsync(string nickName)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDto>> GetUsersAsync(Expression<Func<UserDto, bool>> predicate)
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
