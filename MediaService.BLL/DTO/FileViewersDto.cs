#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class FileViewersDto
    {
        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public Guid FileEntryId { get; set; }

        [DataMember]
        public string UserId { get; set; }

        [DataMember]
        public FileEntryDto FileEntry { get; set; }

        [DataMember]
        public UserDto User { get; set; }
    }
}