using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.DAL.Entities;

namespace MediaService.BLL.Infrastructure
{
    static class MapperModule
    {
        public static IMapper Mapper { get; }

        static MapperModule()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<UserDto, UserProfile>();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>();

                cfg.CreateMap<ObjectEntryDto, ObjectEntry>()
                    .Include<FileEntryDto, FileEntry>()
                    .Include<DirectoryEntryDto, DirectoryEntry>();

                cfg.CreateMap<FileEntryDto, FileEntry>(); ;
                cfg.CreateMap<DirectoryEntryDto, DirectoryEntry>();

                cfg.CreateMap<TagDto, Tag>();
            });

            Mapper = config.CreateMapper();
        }
    }
}
