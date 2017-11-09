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
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<AspNetUserDto> owners = null
            );

        Task<IEnumerable<TObjectDto>> GetByAsync(
            string name = null,
            Guid? parentId = null,
            long? size = null,
            DateTime? created = null,
            DateTime? downloaded = null,
            DateTime? modified = null,
            ICollection<AspNetUserDto> owners = null
        );
    }
}
