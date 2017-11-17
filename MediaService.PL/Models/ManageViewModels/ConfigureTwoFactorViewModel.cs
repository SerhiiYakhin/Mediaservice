#region usings

using System.Collections.Generic;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Models.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
    }
}