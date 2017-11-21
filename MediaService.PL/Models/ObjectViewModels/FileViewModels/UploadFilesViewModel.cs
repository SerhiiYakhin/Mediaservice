using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class UploadFilesViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        [StringLength(200)]
        public HttpFileCollectionBase Files { get; set; }
    }
}