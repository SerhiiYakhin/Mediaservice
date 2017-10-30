using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MS.BusinessLayer.DTO
{
    public sealed class UserDto
    {
        public UserDto() => ObjectEntries = new HashSet<ObjectEntryDto>();

        public Guid Id { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string Nickname { get; set; }

        public string Avatar { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email    { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Role     { get; set; }

        public ICollection<ObjectEntryDto> ObjectEntries { get; set; }
    }
}
