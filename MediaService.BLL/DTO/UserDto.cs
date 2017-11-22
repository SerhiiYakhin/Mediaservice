#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class UserDto
    {
        public UserDto()
        {
            Files = new HashSet<FileEntryDto>();
            Directories = new HashSet<DirectoryEntryDto>();
            SharedFiles = new HashSet<FileViewersDto>();
            SharedDirectories = new HashSet<DirectoryViewersDto>();
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool EmailConfirmed { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public string SecurityStamp { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public bool PhoneNumberConfirmed { get; set; }

        [DataMember]
        public bool TwoFactorEnabled { get; set; }

        [DataMember]
        public DateTime? LockoutEndDateUtc { get; set; }

        [DataMember]
        public bool LockoutEnabled { get; set; }

        [DataMember]
        public int AccessFailedCount { get; set; }


        [DataMember]
        public UserProfileDto UserProfile { get; set; }

        [DataMember]
        public ICollection<FileViewersDto> SharedFiles { get; set; }

        [DataMember]
        public ICollection<DirectoryViewersDto> SharedDirectories { get; set; }

        [DataMember]
        public ICollection<FileEntryDto> Files { get; set; }

        [DataMember]
        public ICollection<DirectoryEntryDto> Directories { get; set; }
    }
}