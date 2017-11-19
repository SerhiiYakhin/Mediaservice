#region usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IFilesService : IObjectService<FileEntryDto>
    {
        Task AddRangeAsync(IEnumerable<FileEntryDto> objects, Guid parentId);
    }
}