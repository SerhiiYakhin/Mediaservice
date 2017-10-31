using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.BLL.Services;
using Ninject;

namespace MediaService.PL.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            //todo: fix this werid hack (check clien model validation)
            _kernel.Unbind<ModelValidatorProvider>();
            //_kernel.Rebind<ModelValidatorProvider>().To<AttributeValidatorProvider>();
            AddBindings();
        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);

        private void AddBindings()
        {
            _kernel.Bind<IObjectService<ObjectEntryDto>>().To<ObjectService<ObjectEntryDto>>();
            _kernel.Bind<IObjectService<FileEntryDto>>().To<ObjectService<FileEntryDto>>();
            _kernel.Bind<IObjectService<DirectoryEntryDto>>().To<ObjectService<DirectoryEntryDto>>();

            _kernel.Bind<ITagService>().To<TagService>();
            _kernel.Bind<IUserService>().To<UserService>();

            //_kernel.Load(Assembly.GetExecutingAssembly());

            //_kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            //_kernel.Bind<IUserStore<ApplicationUser>>().To<ApplicationUserStore>();
            //_kernel.Bind<ApplicationUserManager>().ToSelf();
            //_kernel.Bind<ApplicationSignInManager>().ToSelf();
            //_kernel.Bind<IAuthenticationManager>().ToMethod(x => HttpContext.Current.GetOwinContext().Authentication);
            //_kernel.Bind<IDataProtectionProvider>().ToMethod(x => _app.GetDataProtectionProvider());


            //_kernel.Bind<IUserStore<ApplicationUser>>().To<UserStore<ApplicationUser>>();
            //_kernel.Bind<UserManager<ApplicationUser>>().ToSelf();

            //_kernel.Bind<HttpContextBase>().ToMethod(ctx => new HttpContextWrapper(HttpContext.Current)).InTransientScope();

            //_kernel.Bind<ApplicationSignInManager>().ToMethod((context) =>
            //{
            //    var cbase = new HttpContextWrapper(HttpContext.Current);
            //    return cbase.GetOwinContext().Get<ApplicationSignInManager>();
            //});

            //_kernel.Bind<ApplicationUserManager>().ToSelf();
        }
    }
}