using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class RenameFileViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        [StringLength(128, ErrorMessage = "Max name lenght is 50 symbols")]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}