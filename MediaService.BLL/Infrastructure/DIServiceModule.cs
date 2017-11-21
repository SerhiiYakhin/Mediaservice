#region usings

using MediaService.DAL.Accessors;
using MediaService.DAL.Interfaces;
using Ninject.Modules;

#endregion

namespace MediaService.BLL.Infrastructure
{
    public class DIServiceModule : NinjectModule
    {
        private readonly string _dbConnectionString;
        private readonly string _storageConnection;

        public DIServiceModule(string dbConnection, string storageConnection)
        {
            _dbConnectionString = dbConnection;
            _storageConnection = storageConnection;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_dbConnectionString);
            Bind<IStorage>().To<AzureStorageBlobAccessor>().WithConstructorArgument(_storageConnection);
        }
    }
}