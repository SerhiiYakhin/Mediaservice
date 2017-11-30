using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.DirectoryViewModels
{
    public class DeleteDirectoryViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }
    }
}