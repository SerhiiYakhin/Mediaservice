#region usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace MediaService.PL.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "{0} должно иметь как минимум {2} символа.", MinimumLength = 2)]
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} должен иметь как минимум {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают!")]
        public string ConfirmPassword { get; set; }
    }
}