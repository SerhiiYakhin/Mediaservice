#region usings

using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using MediaService.BLL.DTO.Enums;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class FileEntryDto : ObjectEntryDto
    {
        public FileEntryDto()
        {
            Viewers = new HashSet<FileViewersDto>();
            Tags = new HashSet<TagDto>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Size { get; set; }

        [DataMember]
        public FileType FileType { get; set; }

        public Stream FileStream { get; set; }

        [DataMember]
        public string FileThumbnailLink { get; set; }

        [DataMember]
        public string OwnerId { get; set; }

        [DataMember]
        public virtual UserDto Owner { get; set; }

        [DataMember]
        public ICollection<FileViewersDto> Viewers { get; set; }

        [DataMember]
        public ICollection<TagDto> Tags { get; set; }
    }
}