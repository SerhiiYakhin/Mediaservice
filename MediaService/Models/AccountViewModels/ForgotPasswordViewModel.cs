using System.ComponentModel.DataAnnotations;

namespace MediaService.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        [Display(Name = "Nickname")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}