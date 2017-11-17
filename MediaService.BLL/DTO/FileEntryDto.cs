using System;
using System.Collections.Generic;
using System.IO;

namespace MediaService.BLL.DTO
{
    public class FileEntryDto : ObjectEntryDto
    {
        public FileEntryDto()
        {
            Viewers = new HashSet<FileViewersDto>();
            Tags = new HashSet<TagDto>();
        }

        public int Size { get; set; }

        public Stream FileStream { get; set; }

        public string OwnerId { get; set; }

        public virtual UserDto Owner { get; set; }

        public virtual ICollection<FileViewersDto> Viewers { get; set; }

        public virtual ICollection<TagDto> Tags { get; set; }
    }
}
