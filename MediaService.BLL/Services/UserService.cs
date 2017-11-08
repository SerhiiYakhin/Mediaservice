using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public sealed class UserService : Service<UserDto, UserProfile, string>, IUserService
    {
        public UserService(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
