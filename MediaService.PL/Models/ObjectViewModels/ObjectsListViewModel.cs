#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MediaService.PL.Models.ObjectViewModels.Enums;

#endregion

namespace MediaService.PL.Models.ObjectViewModels
{
    public class ObjectsListViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public Guid? ParentId { get; set; }

        [Required]
        public OrderType OrderType { get; set; }
    }
}