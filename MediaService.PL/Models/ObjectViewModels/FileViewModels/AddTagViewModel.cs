using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class AddTagViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid FileId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }
    }
}