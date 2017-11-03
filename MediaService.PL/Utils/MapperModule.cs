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
                cfg.CreateMap<RegisterViewModel, UserDto>().ReverseMap();
                cfg.CreateMap<ApplicationUser, UserDto>().ForMember(d => d.Nickname, opt => opt.MapFrom(src => src.UserName));
            });
        }
    }
}