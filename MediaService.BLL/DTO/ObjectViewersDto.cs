using System;

namespace MediaService.BLL.DTO
{
    public class ObjectViewersDto
    {
        public string Link { get; set; }

        public Guid ObjectEntryId { get; set; }

        public string UserId { get; set; }

        public virtual ObjectEntryDto ObjectEntry { get; set; }

        public virtual UserDto User { get; set; }
    }
}
