#region usings

using System;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IUserProfileService : IService<UserProfileDto, Guid>
    {
    }
}