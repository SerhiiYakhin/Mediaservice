using System;
using System.Web;
using System.Web.Mvc;
using MediaService.BLL.Infrastructure;
using Ninject;
using Ninject.Web.Common;

using MediaService.PL.Utils;

namespace MediaService.PL
{
    public partial class Startup
    {
        public void ConfigureNinject()
        {
            //todo: Make correct variant for try..catch dependency injection
            var kernel = new StandardKernel(new ServiceModule("DefaultConnection"));
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
    }
}