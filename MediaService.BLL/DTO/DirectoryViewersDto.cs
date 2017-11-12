using System;

namespace MediaService.BLL.DTO
{
    public class DirectoryViewersDto
    {
        public string Link { get; set; }

        public Guid FileEntryId { get; set; }

        public string UserId { get; set; }

        public virtual FileEntryDto FileEntry { get; set; }

        public virtual UserDto User { get; set; }
    }
}
