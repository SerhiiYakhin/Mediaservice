#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Models.ObjectViewModels.DirectoryViewModels
{
    public class CreateDirectoryViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Max name lenght is 50 symbols")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}