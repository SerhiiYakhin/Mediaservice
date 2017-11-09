using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class ApplicationUserService : Service<AspNetUserDto, AspNetUser, string>, IApplicationUserService
    {
        public ApplicationUserService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.AspNetUsers;
        }
    }
}
