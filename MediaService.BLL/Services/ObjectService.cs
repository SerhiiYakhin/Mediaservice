using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class ObjectService<TEntity> : Service<TEntity, Guid>, IObjectService<TEntity> where TEntity : ObjectEntryDto
    {
        public IEnumerable<TEntity> GetBy(string name = null, Guid? parentId = null, long? size = null, DateTime? created = null, DateTime? downloaded = null, DateTime? modified = null, ICollection<UserDto> owners = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByAsync(string name = null, Guid? parentId = null, long? size = null, DateTime? created = null, DateTime? downloaded = null, DateTime? modified = null, ICollection<UserDto> owners = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetByParentId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByParentIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public ObjectService(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
