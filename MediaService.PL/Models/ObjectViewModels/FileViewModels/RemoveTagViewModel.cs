using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class RemoveTagViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid FileId { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid TagId { get; set; }
    }
}