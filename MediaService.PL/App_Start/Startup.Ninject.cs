#region usings

using System;
using System.Web;
using System.Web.Mvc;
using MediaService.BLL.Infrastructure;
using MediaService.PL.Utils;
using Microsoft.Azure;
using Ninject;
using Ninject.Web.Common;

#endregion

namespace MediaService.PL
{
    public partial class Startup
    {
        private void ConfigureNinject()
        {
            var kernel = new StandardKernel(new ServiceModule("DefaultConnection", CloudConfigurationManager.GetSetting("StorageConnectionString")));
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            }
            catch (Exception ex)
            {
                kernel.Dispose();
                DependencyResolver.SetResolver(new DependencyResolver());
                throw new Exception("Error while setting NinjectDependencyResolver", ex);
            }
        }
    }
}