#region usings

using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.PL.Models.AccountViewModels;
using MediaService.PL.Models.IdentityModels;
using MediaService.PL.Models.ObjectViewModels;
using MediaService.PL.Models.ObjectViewModels.DirectoryViewModels;
using MediaService.PL.Models.ObjectViewModels.FileViewModels;

#endregion

namespace MediaService.PL.Utils
{
    public static class MapperModule
    {
        private static readonly MapperConfiguration Config;

        static MapperModule()
        {
            Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ExternalLoginConfirmationViewModel, ApplicationUser>();
                cfg.CreateMap<RegisterViewModel, ApplicationUser>();
                cfg.CreateMap<ApplicationUser, UserDto>();
                cfg.CreateMap<CreateDirectoryViewModel, DirectoryEntryDto>();
                cfg.CreateMap<RenameFileViewModel, FileEntryDto>();
                cfg.CreateMap<RenameDirectoryViewModel, DirectoryEntryDto>();

                //cfg.CreateMap<AddTagViewModel, TagDto>();
            });
        }

        public static IMapper GetMapper()
        {
            return Config.CreateMapper();
        }
    }
}