using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace MediaService.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool                 HasPassword       { get; set; }
        public bool                 TwoFactor         { get; set; }
        public bool                 BrowserRemembered { get; set; }
        public string               PhoneNumber       { get; set; }
        public IList<UserLoginInfo> Logins            { get; set; }
    }
}