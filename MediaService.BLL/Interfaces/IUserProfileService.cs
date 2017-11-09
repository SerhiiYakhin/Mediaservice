using MediaService.BLL.DTO;
using System;

namespace MediaService.BLL.Interfaces
{
    public interface IUserProfileService: IService<UserProfileDto, Guid>
    {
    }
}