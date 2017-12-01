#region usings

using System;
using System.Web;
using System.Web.Mvc;
using MediaService.PL.Utils;
using Microsoft.Azure;
using Ninject;
using Ninject.Web.Common;
using MediaService.BLL.Infrastructure;

#endregion

namespace MediaServiceBLLJob
{
    public class Startup
    {
        public Startup()
        {
            ConfigureNinject();
        }

        private void ConfigureNinject()
        {
            var kernel = new StandardKernel(new DIServiceModule("DefaultConnection", CloudConfigurationManager.GetSetting("StorageConnectionString")));

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

        public IDependencyResolver GetCurrect()
        {
            return DependencyResolver.Current;
        }
    }
}