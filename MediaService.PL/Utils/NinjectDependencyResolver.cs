using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.BLL.Services;
using MediaService.BLL.Services.ObjectsServices;
using Ninject;

namespace MediaService.PL.Utils
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            _kernel = kernelParam;
            _kernel.Unbind<ModelValidatorProvider>();
            AddBindings();
        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);

        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);

        private void AddBindings()
        {
            _kernel.Bind<IObjectService<ObjectEntryDto>>().To<ObjectService>();
            _kernel.Bind<IObjectService<FileEntryDto>>().To<FileService>();
            _kernel.Bind<IObjectService<DirectoryEntryDto>>().To<DirectoryService>();

            _kernel.Bind<ITagService>().To<TagService>();
            _kernel.Bind<IUserService>().To<UserService>();
        }
    }
}