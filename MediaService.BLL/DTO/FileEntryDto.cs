using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public class FileEntryDto : ObjectEntryDto
    {
        public FileEntryDto() => Tags = new HashSet<TagDto>();

        public int Size { get; set; }

        public virtual ICollection<TagDto> Tags { get; set; }
    }
}
