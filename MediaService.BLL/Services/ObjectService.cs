using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class ObjectService<TEntity> : IObjectService<TEntity> where TEntity : ObjectEntryDto
    {
        private IUnitOfWork Database { get; }

        public ObjectService(IUnitOfWork uow) => Database = uow;

        public TEntity GetObject(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetObjects()
        {
            throw new NotImplementedException();
        }

        public OperationDetails EditObject(TEntity item)
        {
            throw new NotImplementedException();
        }

        public OperationDetails CreateObjects(params TEntity[] list)
        {
            throw new NotImplementedException();
        }

        public OperationDetails DeleteObjects(params TEntity[] list)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> GetObjectAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TEntity>> GetObjectsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> EditObjectAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> CreateObjectsAsync(params TEntity[] list)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> DeleteObjectsAsync(params TEntity[] list)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => Database.Dispose();
    }
}
