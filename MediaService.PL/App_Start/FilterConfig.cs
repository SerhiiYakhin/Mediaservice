#region usings

using System.Web.Mvc;
using MediaService.PL.Utils.Attributes.ErrorHandler;

#endregion

namespace MediaService.PL
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AiHandleErrorAttribute());
        }
    }
}