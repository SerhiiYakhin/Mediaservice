using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public sealed class UserDto
    {
        public UserDto() => ObjectEntries = new HashSet<ObjectEntryDto>();

        public string Id       { get; set; }

        public string Nickname { get; set; }

        public string Avatar   { get; set; }

        public ICollection<ObjectEntryDto> ObjectEntries { get; set; }
    }
}
