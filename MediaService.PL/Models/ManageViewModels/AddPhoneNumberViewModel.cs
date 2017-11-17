#region usings

using System.ComponentModel.DataAnnotations;

#endregion

namespace MediaService.PL.Models.ManageViewModels
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }
}