#region usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IObjectService<TObjectDto> : IService<TObjectDto, Guid> where TObjectDto : ObjectEntryDto
    {
        Task<IEnumerable<TObjectDto>> GetByParentIdAsync(Guid id);
        
        Task<IEnumerable<TObjectDto>> GetByAsync(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            string ownerId = null
        );

        Task<bool> ExistAsync(string name, Guid parentId);
    }
}