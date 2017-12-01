#region usings

using MediaService.BLL.Interfaces;
using MediaService.BLL.Services.ObjectsServices;
using MediaService.BLL.Services.UserServices;
using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

#endregion

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

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<IFileService>().To<FileService>();
            _kernel.Bind<IDirectoryService>().To<DirectoryService>();

            _kernel.Bind<ITagService>().To<TagService>();
            _kernel.Bind<IUserProfileService>().To<UserProfileService>();
            _kernel.Bind<IUserService>().To<UserService>();
        }
    }
}