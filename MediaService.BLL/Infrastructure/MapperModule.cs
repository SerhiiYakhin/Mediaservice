using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.DAL.Entities;

namespace MediaService.BLL.Infrastructure
{
    static class MapperModule
    {
        private static readonly MapperConfiguration Config;

        public static IMapper GetMapper() => Config.CreateMapper();

        static MapperModule()
        {
            Config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDto, UserProfile>().ReverseMap();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>().ReverseMap();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>()
                    .Include<FileEntryDto, FileEntry>()
                    .Include<DirectoryEntryDto, DirectoryEntry>().ReverseMap();

                cfg.CreateMap<FileEntryDto, FileEntry>().ReverseMap();
                cfg.CreateMap<DirectoryEntryDto, DirectoryEntry>().ReverseMap();

                cfg.CreateMap<TagDto, Tag>().ReverseMap();
            });
        }
    }
}
