using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public class UserDto
    {
        public UserDto()
        {
            Objects = new HashSet<ObjectEntryDto>();
            SharedObjects = new HashSet<ObjectViewersDto>();
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

        public virtual ICollection<ObjectViewersDto> SharedObjects { get; set; }

        public virtual ICollection<ObjectEntryDto> Objects { get; set; }
    }
}
