#region usings

using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IUserService : IService<UserDto, string>
    {
    }
}