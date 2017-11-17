#region usings

using System;
using System.Web.Mvc;

#endregion

namespace MediaService.PL.Utils.Attributes.ErrorHandler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class ErrorHandleAttribute : HandleErrorAttribute
    {
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext?.Exception == null
                || filterContext.ExceptionHandled
                || !filterContext.HttpContext.IsCustomErrorEnabled
            )
            {
                return;
            }

            if (IsAjax(filterContext))
            {
                filterContext.Result = new JsonResult
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                base.OnException(filterContext);
            }
        }
    }
}