#region usings

using System;
using System.Threading.Tasks;
using Microsoft.Owin;

#endregion

namespace MediaService.PL.Utils.Attributes.ErrorHandler
{
    public class FailedSetupMiddleware
    {
        private readonly Exception _exception;

        public FailedSetupMiddleware(Exception exception)
        {
            _exception = exception;
        }

        //@todo: Make correct Redirect with good html error view
        public Task Invoke(IOwinContext context, Func<Task> next)
        {
            var message = "The service is unavailable because there was an error during application start";
            context.Response.StatusCode = 500;
            //context.Response.Redirect("/Shared/Error");
            return context.Response.WriteAsync(message);
        }
    }
}