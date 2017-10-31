using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;

namespace MediaService.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        UserDto GetUserById(string id);
        UserDto GetUserByNick(string nickName);
        IEnumerable<UserDto> GetUsers(Expression<Func<UserDto, bool>> predicate);
        OperationDetails EditUser(UserDto item);
        void CreateUser(UserDto userDto);
        void DeleteUser(UserDto userDto);

        Task<UserDto> GetUserByIdAsync(string id);
        Task<UserDto> GetUserByNickAsync(string nickName);
        Task<IEnumerable<UserDto>> GetUsersAsync(Expression<Func<UserDto, bool>> predicate);
        Task<OperationDetails> EditUserAsync(UserDto item);
        Task CreateUserAsync(UserDto userDto);
        Task DeleteUserAsync(UserDto userDto);
    }
}
