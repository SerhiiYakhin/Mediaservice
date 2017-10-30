using Ninject.Modules;
using MediaService.DAL.Interfaces;
using MediaService.DAL.Repositories;

namespace MediaService.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private readonly string _connectionString;

        public ServiceModule(string connection) => _connectionString = connection;

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_connectionString);
        }
    }
}
