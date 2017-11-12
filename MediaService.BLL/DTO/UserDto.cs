using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public class UserDto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDto()
        {
            Files = new HashSet<FileEntryDto>();
            Directories = new HashSet<DirectoryEntryDto>();
            SharedFiles = new HashSet<FileViewersDto>();
            SharedDirectories = new HashSet<DirectoryViewersDto>();
        }

        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }


        public virtual UserProfileDto UserProfile { get; set; }

        public virtual ICollection<FileViewersDto> SharedFiles { get; set; }

        public virtual ICollection<DirectoryViewersDto> SharedDirectories { get; set; }

        public virtual ICollection<FileEntryDto> Files { get; set; }

        public virtual ICollection<DirectoryEntryDto> Directories { get; set; }
    }
}
