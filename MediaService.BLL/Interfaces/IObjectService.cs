using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;

namespace MediaService.BLL.Interfaces
{
    public interface IObjectService<TEntity> : IDisposable where TEntity : ObjectEntryDto
    {
        TEntity GetObject(Guid? id);
        IEnumerable<TEntity> GetObjects();
        OperationDetails EditObject(TEntity item);
        OperationDetails CreateObjects(params TEntity[] list);
        OperationDetails DeleteObjects(params TEntity[] list);

        Task<TEntity> GetObjectAsync(Guid? id);
        Task<IEnumerable<TEntity>> GetObjectsAsync();
        Task<OperationDetails> EditObjectAsync(TEntity item);
        Task<OperationDetails> CreateObjectsAsync(params TEntity[] list);
        Task<OperationDetails> DeleteObjectsAsync(params TEntity[] list);
    }
}
