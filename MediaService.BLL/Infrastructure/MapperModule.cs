#region usings

using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.DAL.Entities;

#endregion

namespace MediaService.BLL.Infrastructure
{
    internal static class MapperModule
    {
        private static readonly MapperConfiguration Config;

        static MapperModule()
        {
            Config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserProfileDto, UserProfile>().ReverseMap();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>().ReverseMap();

                cfg.CreateMap<UserDto, User>().ReverseMap();

                cfg.CreateMap<FileViewersDto, FileViewers>().ReverseMap();
                cfg.CreateMap<DirectoryViewersDto, DirectoryViewers>().ReverseMap();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>()
                    .Include<FileEntryDto, FileEntry>()
                    .Include<DirectoryEntryDto, DirectoryEntry>().ReverseMap();

                cfg.CreateMap<FileEntryDto, FileEntry>().ReverseMap();
                cfg.CreateMap<DirectoryEntryDto, DirectoryEntry>().ReverseMap();

                cfg.CreateMap<TagDto, Tag>().ReverseMap();
            });
        }

        public static IMapper GetMapper()
        {
            return Config.CreateMapper();
        }
    }
}