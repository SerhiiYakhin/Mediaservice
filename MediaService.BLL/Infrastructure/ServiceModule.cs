using System.Collections.Generic;
using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.DAL.Entities;
using Ninject.Modules;
using MediaService.DAL.Interfaces;
using MediaService.DAL.Repositories;

namespace MediaService.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        //private static Action<IMapperConfigurationExpression> _dtoToEntityCfg = cfg => cfg.CreateMap<UserDto, UserProfile>();

        //private static Action<IMapperConfigurationExpression> _entityToDtoCfg = cfg => cfg.CreateMap<UserDto, UserProfile>();
        private readonly string _connectionString;

        public ServiceModule(string connection)
        {
            _connectionString = connection;
            //InitMapper();
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(_connectionString);
        }

        private void InitMapper()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<UserDto, UserProfile>());
            Mapper.Initialize(cfg => cfg.CreateMap<UserProfile, UserDto>());
            Mapper.Initialize(cfg => cfg.CreateMap<IEnumerable<UserProfile>, IEnumerable<UserDto>>());
            Mapper.Initialize(cfg => cfg.CreateMap<IEnumerable<UserDto>, IEnumerable<UserProfile>>());
        }
    }
}
