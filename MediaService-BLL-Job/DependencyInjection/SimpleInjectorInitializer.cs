#region usings

using MediaService.BLL.Interfaces;
using MediaService.BLL.Services.ObjectsServices;
using MediaService.BLL.Services.UserServices;
using MediaService.DAL.Accessors;
using MediaService.DAL.Interfaces;
using Microsoft.Azure;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

#endregion

namespace MediaService_BLL_Job.DependencyInjection
{
    /// <summary>
    ///     Simple Injector Initializer
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container.</summary>
        public static Container Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            //container.Options.DefaultScopedLifestyle = Lifestyle.Scoped;

            InitializeContainer(container);

            container.Verify();

            return container;
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<IFileService, FileService>(Lifestyle.Scoped);
            container.Register<IDirectoryService, DirectoryService>(Lifestyle.Scoped);
            container.Register<ITagService, TagService>(Lifestyle.Scoped);

            container.Register<IUserProfileService, UserProfileService>(Lifestyle.Scoped);
            container.Register<IUserService, UserService>(Lifestyle.Scoped);
            container.Register<IUnitOfWork>(() => new EFUnitOfWork("DefaultConnection"), Lifestyle.Scoped);
            container.Register<IQueueStorage>(
                () => new AzureStorageQueueAccessor(CloudConfigurationManager.GetSetting("StorageConnectionString")),
                Lifestyle.Scoped);
            container.Register<IBlobStorage>(
                () => new AzureStorageBlobAccessor(CloudConfigurationManager.GetSetting("StorageConnectionString")),
                Lifestyle.Scoped);

            // Singleton Example
            // container.RegisterSingleton<ILogger, Logger>();

            // Scoped Example
            // container.Register<EntityFrameworkContext>(Lifestyle.Scoped);
        }
    }
}