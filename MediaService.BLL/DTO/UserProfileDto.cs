using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public class UserProfileDto
    {
        public UserProfileDto() => AppUsers = new HashSet<AspNetUserDto>();

        public Guid Id { get; set; }

        public string Avatar { get; set; }
        
        public virtual ICollection<AspNetUserDto> AppUsers { get; set; }
    }
}
