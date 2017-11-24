using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MediaService.BLL.DTO.Enums;
using MediaService.PL.Models.ObjectViewModels.Enums;

namespace MediaService.PL.Models.ObjectViewModels.FileViewModels
{
    public class SearchFilesViewModel
    {
        [Required]
        [HiddenInput(DisplayValue = false)]
        public Guid ParentId { get; set; }

        [Required]
        public OrderType OrderType { get; set; }

        [Required]
        public SearchType SearchType { get; set; }

        [Required]
        [StringLength(50)]
        public string SearchValue { get; set; }
    }
}