using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using Ninject;
using Ninject.Web.Common;

using MediaService.Util;
using MS.BusinessLayer.Infrastructure;

namespace MediaService
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //todo: Make correct variant for try..catch dependency injection
            var kernel = new StandardKernel(new ServiceModule("DefaultConnection", Startup.AppName));
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
