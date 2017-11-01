using AutoMapper;
using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AutoMapper.Mapper;

namespace MediaService.BLL.Services
{
    public class UserService : IUserService
    {
        private IRuntimeMapper _dtoToEntity;

        private IRuntimeMapper _entityToDto;

        //private static Action<IMapperConfigurationExpression> _dtoToEntityCfg = cfg => cfg.CreateMap<UserDto, UserProfile>();

        //private static Action<IMapperConfigurationExpression> _entityToDtoCfg = cfg => cfg.CreateMap<UserDto, UserProfile>();

        //private static Action<IMapperConfigurationExpression> DtoToEntityCfg => _dtoToEntityCfg ?? (_dtoToEntityCfg = cfg => cfg.CreateMap<UserDto, UserProfile>());

        //private static Action<IMapperConfigurationExpression> EntityToDtoCfg => _entityToDtoCfg ?? (_entityToDtoCfg = cfg => cfg.CreateMap<UserProfile, UserDto>());

        private IRuntimeMapper DtoToEntity => _dtoToEntity ?? (_dtoToEntity =
                                                  new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserDto, UserProfile>()))
                                                      .DefaultContext
                                                      .Mapper);

        private IRuntimeMapper EntityToDto => _entityToDto ?? (_entityToDto =
                                                  new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDto>()))
                                                      .DefaultContext
                                                      .Mapper);

        private IUnitOfWork Database { get; }

        public UserService(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public UserDto FindById(string id)
        {
            return EntityToDto.Map<UserDto>(Database.Users.FindByKey(id));
        }

        public async Task<UserDto> FindByIdAsync(string id)
        {
            return EntityToDto.Map<UserDto>(await Database.Users.FindByKeyAsync(id));
        }

        public IEnumerable<UserDto> GetData()
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserProfile>, IEnumerable<UserDto>>());
            return Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(Database.Users.GetData());
        }

        public async Task<IEnumerable<UserDto>> GetDataAsync()
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserProfile>, IEnumerable<UserDto>>());
            return Map<IEnumerable<UserProfile>, IEnumerable<UserDto>>(await Database.Users.GetDataAsync());
        }

        public void Add(UserDto item)
        {
            Database.Users.Add(DtoToEntity.Map<UserProfile>(item));
        }

        public async Task AddAsync(UserDto item)
        {
            await Database.Users.AddAsync(DtoToEntity.Map<UserProfile>(item));
        }

        public void AddRange(IEnumerable<UserDto> items)
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserDto>, IEnumerable<UserProfile>>());
            Database.Users.AddRange(Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
        }

        public async Task AddRangeAsync(IEnumerable<UserDto> items)
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserDto>, IEnumerable<UserProfile>>());
            await Database.Users.AddRangeAsync(Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
        }

        public void Update(UserDto item)
        {
            Database.Users.Update(DtoToEntity.Map<UserProfile>(item));
        }

        public async Task UpdateAsync(UserDto item)
        {
            await Database.Users.UpdateAsync(DtoToEntity.Map<UserProfile>(item));
        }

        public void Remove(UserDto item)
        {
            Database.Users.Remove(DtoToEntity.Map<UserProfile>(item));
        }

        public async Task RemoveAsync(UserDto item)
        {
            await Database.Users.RemoveAsync(DtoToEntity.Map<UserProfile>(item));
        }

        public void RemoveRange(IEnumerable<UserDto> items)
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserDto>, IEnumerable<UserProfile>>());
            Database.Users.RemoveRange(Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
        }

        public async Task RemoveRangeAsync(IEnumerable<UserDto> items)
        {
            Initialize(cfg => cfg.CreateMap<IEnumerable<UserDto>, IEnumerable<UserProfile>>());
            await Database.Users.RemoveRangeAsync(Map<IEnumerable<UserDto>, IEnumerable<UserProfile>>(items));
        }

        public UserDto GetUserByNick(string nickName)
        {
            return EntityToDto.Map<UserDto>(Database.Users.GetData(u => u.Nickname.Equals(nickName)).SingleOrDefault());
        }

        public async Task<UserDto> GetUserByNickAsync(string nickName)
        {
            return EntityToDto.Map<UserDto>((await Database.Users.GetDataAsync(u => u.Nickname.Equals(nickName))).SingleOrDefault());
        }
    }
}
