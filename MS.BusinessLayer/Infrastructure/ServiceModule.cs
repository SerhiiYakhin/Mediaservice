using Ninject.Modules;
using MS.DataLayer.Interfaces;
using MS.DataLayer.Repositories;

namespace MS.BusinessLayer.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;
        private readonly string _appName;

        public ServiceModule(string connection, string appName)
        {
            _connectionString = connection;
            _appName = appName;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_connectionString, _appName);
        }
    }
}
