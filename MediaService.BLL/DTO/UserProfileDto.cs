#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class UserProfileDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Avatar { get; set; }

        [DataMember]
        public UserDto User { get; set; }
    }
}