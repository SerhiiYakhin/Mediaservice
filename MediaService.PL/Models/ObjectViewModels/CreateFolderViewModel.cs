#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Models.ObjectViewModels
{
    public class CreateFolderViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}