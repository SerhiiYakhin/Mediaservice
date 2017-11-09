using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;

namespace MediaService.BLL.Services
{
    public sealed class UserProfileService : Service<UserProfileDto, UserProfile, Guid>, IUserProfileService
    {
        public UserProfileService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.UsersProfiles;
        }
    }
}
