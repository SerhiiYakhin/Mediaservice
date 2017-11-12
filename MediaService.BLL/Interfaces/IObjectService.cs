using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

namespace MediaService.BLL.Interfaces
{
    public interface IObjectService<TObjectDto> : IService<TObjectDto, Guid> where TObjectDto : ObjectEntryDto
    {
        IEnumerable<TObjectDto> GetByName(string name);

        Task<IEnumerable<TObjectDto>> GetByNameAsync(string name);


        IEnumerable<TObjectDto> GetByParentId(Guid id);

        Task<IEnumerable<TObjectDto>> GetByParentIdAsync(Guid id);


        IEnumerable<TObjectDto> GetBy(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            UserDto owner = null
            );

        Task<IEnumerable<TObjectDto>> GetByAsync(
            Guid? id = null,
            string name = null,
            Guid? parentId = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            UserDto owner = null
        );
    }
}
