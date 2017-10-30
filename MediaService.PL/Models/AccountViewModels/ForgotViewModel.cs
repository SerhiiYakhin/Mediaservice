using System.ComponentModel.DataAnnotations;

namespace MediaService.PL.Models.AccountViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}