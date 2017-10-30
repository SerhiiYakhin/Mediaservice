using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public sealed class FileEntryDto : ObjectEntryDto
    {
        public FileEntryDto() => Tags = new HashSet<TagDto>();

        public ICollection<TagDto> Tags { get; set; }
    }
}
