using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;

namespace MediaService.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        UserDto GetUserById(string id);
        bool UserExist(Func<UserDto, bool> predicate);
        IEnumerable<UserDto> GetUsers(Predicate<UserDto> predicate);
        OperationDetails EditUser(UserDto item);
        void CreateUser(UserDto userDto);
        void DeleteUser(UserDto userDto);

        Task<UserDto> GetUserByIdAsync(string id);
        Task<IEnumerable<UserDto>> GetUsersAsync(Predicate<UserDto> predicate);
        Task<OperationDetails> EditUserAsync(UserDto item);
        Task CreateUserAsync(UserDto userDto);
        Task DeleteUserAsync(UserDto userDto);
    }
}
