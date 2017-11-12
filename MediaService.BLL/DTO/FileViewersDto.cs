using System;

namespace MediaService.BLL.DTO
{
    public class FileViewersDto
    {
        public string Link { get; set; }

        public Guid DirectoryEntryId { get; set; }

        public string UserId { get; set; }

        public virtual DirectoryEntryDto DirectoryEntry { get; set; }

        public virtual UserDto User { get; set; }
    }
}
