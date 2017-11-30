#region usings

using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.BLL.Services.UserServices
{
    public sealed class UserProfileService : Service<UserProfileDto, UserProfile, Guid>, IUserProfileService
    {
        public UserProfileService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.UsersProfiles;
        }

        public override void Add(UserProfileDto item)
        {
            var userProfile = DtoMapper.Map<UserProfile>(item);
            var user = Context.Users.FindByKey(userProfile.User.Id);

            if (user == null)
            {
                throw new InvalidDataException("There is no such user in database, adding UserProfile impossible");
            }

            if(user.UserProfile == null)
            {
                userProfile.User = user;
                Context.UsersProfiles.Add(userProfile);
                Context.SaveChanges();
            }
            else
            {
                throw new InvalidExpressionException(
                    "This user already have an UserProfile, maybe you want to Update it");
            }
        }

        public override async Task AddAsync(UserProfileDto item)
        {
            var userProfile = DtoMapper.Map<UserProfile>(item);
            var user = await Context.Users.FindByKeyAsync(userProfile.User.Id);

            if (user == null)
            {
                throw new InvalidDataException("There is no such user in database, adding UserProfile impossible");
            }

            if (user.UserProfile == null)
            {
                userProfile.User = user;
                await Context.UsersProfiles.AddAsync(userProfile);
                await Context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidExpressionException(
                    "This user already have an UserProfile, maybe you want to Update it");
            }
        }
    }
}