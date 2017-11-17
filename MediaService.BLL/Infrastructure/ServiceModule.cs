using MediaService.DAL.Accessors;
using MediaService.DAL.Interfaces;
using Ninject.Modules;

namespace MediaService.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _dbConnectionString;
        private readonly string _storageConnection;

        public ServiceModule(string dbConnection, string storageConnection)
        {
            _dbConnectionString = dbConnection;
            _storageConnection = storageConnection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_dbConnectionString);
            Bind<IStorage>().To<AzureStorageAccessor>().WithConstructorArgument(_storageConnection);
        }
    }
}
