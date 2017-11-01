using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class ObjectService<TEntity> : IObjectService<TEntity> where TEntity : class
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public TEntity FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetData()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetDataAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void Remove(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TEntity> items)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<TEntity> items)
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

        public IEnumerable<TEntity> GetBy(string name = null, Guid? parentId = null, long? size = null, DateTime? created = null,
            DateTime? downloaded = null, DateTime? modified = null, ICollection<UserDto> owners = null)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetByAsync(string name = null, Guid? parentId = null, long? size = null, DateTime? created = null,
            DateTime? downloaded = null, DateTime? modified = null, ICollection<UserDto> owners = null)
        {
            throw new NotImplementedException();
        }
    }
}
