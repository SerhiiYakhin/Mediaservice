using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.PL.Models.AccountViewModels;

namespace MediaService.PL.Utils
{
    public static class MapperModule
    {
        public static IMapper Mapper { get; }

        static MapperModule()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RegisterViewModel, UserDto>();
            });

            Mapper = config.CreateMapper();
        }
    }
}