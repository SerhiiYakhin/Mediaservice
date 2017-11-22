#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class DirectoryViewersDto
    {
        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public Guid DirectoryEntryId { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public DirectoryEntryDto DirectoryEntry { get; set; }

        [DataMember]
        public UserDto User { get; set; }
    }
}