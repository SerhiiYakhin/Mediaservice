#region usings
using Microsoft.ApplicationInsights;
using System;using System.Web.Mvc;
#endregion
namespace MediaService.PL.Utils.Attributes.ErrorHandler{    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]    public class AiHandleErrorAttribute : HandleErrorAttribute    {        public override void OnException(ExceptionContext filterContext)        {            if (filterContext?.HttpContext != null && filterContext.Exception != null)            {                if (filterContext.HttpContext.IsCustomErrorEnabled)                {                    var ai = new TelemetryClient();                    ai.TrackException(filterContext.Exception.InnerException ?? filterContext.Exception);                }            }            base.OnException(filterContext);        }    }}