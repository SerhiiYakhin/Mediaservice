#region usings

using System;

#endregion

namespace MediaService.BLL.DTO
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }

        public string Avatar { get; set; }

        public virtual UserDto User { get; set; }
    }
}