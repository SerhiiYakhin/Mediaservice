#region usings

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class DirectoryEntryDto : ObjectEntryDto
    {
        public DirectoryEntryDto()
        {
            Viewers = new HashSet<DirectoryViewersDto>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public short NodeLevel { get; set; }

        [DataMember]
        public string OwnerId { get; set; }

        [DataMember]
        public UserDto Owner { get; set; }

        [DataMember]
        public ICollection<DirectoryViewersDto> Viewers { get; set; }
    }
}