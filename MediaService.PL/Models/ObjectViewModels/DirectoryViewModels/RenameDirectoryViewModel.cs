#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Models.ObjectViewModels.DirectoryViewModels
{
    public class RenameDirectoryViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}