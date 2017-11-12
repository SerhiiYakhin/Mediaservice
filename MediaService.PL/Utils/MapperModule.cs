using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.PL.Models.AccountViewModels;
using MediaService.PL.Models.IdentityModels;

namespace MediaService.PL.Utils
{
    public static class MapperModule
    {
        private static readonly MapperConfiguration Config;

        public static IMapper GetMapper() => Config.CreateMapper();

        static MapperModule()
        {
            Config = new MapperConfiguration(cfg => {
                cfg.CreateMap<ExternalLoginConfirmationViewModel, ApplicationUser>();
                cfg.CreateMap<RegisterViewModel, ApplicationUser>();
                cfg.CreateMap<ApplicationUser, UserDto>();
            });
        }
    }
}