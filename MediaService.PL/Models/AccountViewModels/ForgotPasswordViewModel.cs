#region usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace MediaService.PL.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}