using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

namespace MediaService.BLL.Interfaces
{
    public interface IObjectService<TEntity> : IService<TEntity, Guid> where TEntity : class
    {
        IEnumerable<TEntity> GetByName(string name);

        Task<IEnumerable<TEntity>> GetByNameAsync(string name);


        IEnumerable<TEntity> GetByParentId(Guid id);

        Task<IEnumerable<TEntity>> GetByParentIdAsync(Guid id);


        IEnumerable<TEntity> GetBy(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
            );

        Task<IEnumerable<TEntity>> GetByAsync(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<UserDto> owners = null
        );
    }
}
