using System.Threading.Tasks;
using MediaService.BLL.DTO;

namespace MediaService.BLL.Interfaces
{
    public interface IUserService: IService<UserDto, string>
    {
        UserDto GetUserByNick(string nickName);

        Task<UserDto> GetUserByNickAsync(string nickName);
    }
}