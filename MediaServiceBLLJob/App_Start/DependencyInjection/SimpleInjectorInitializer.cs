using MediaService.BLL.Interfaces;
using MediaService.BLL.Services.ObjectsServices;
using MediaService.BLL.Services.UserServices;
using SimpleInjector;

namespace MediaServiceBLLJob.App_Start.DependencyInjection
{
    /// <summary>
    /// Simple Injector Initializer
    /// </summary>
    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container.</summary>
        public static Container Initialize()
        {
            var container = new Container();
            //container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            container.Options.DefaultScopedLifestyle = Lifestyle.Scoped;

            InitializeContainer(container);

            container.Verify();

            return container;
        }

        private static void InitializeContainer(Container container)
        {
            container.Register<IFileService, FileService>();
            container.Register<IDirectoryService, DirectoryService>();

            container.Register<ITagService, TagService>();
            container.Register<IUserProfileService, UserProfileService>();
            container.Register<IUserService, UserService>();

            // Singleton Example
            // container.RegisterSingleton<ILogger, Logger>();

            // Scoped Example
            // container.Register<EntityFrameworkContext>(Lifestyle.Scoped);
        }
    }
}