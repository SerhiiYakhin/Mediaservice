using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.DirectoryViewModels
{
    public class DownloadDirectoryViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}