using System;
using MediaService.BLL.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaService.BLL.Interfaces
{
    public interface IFilesService : IObjectService<FileEntryDto>
    {
        Task AddFilesAsync(List<FileEntryDto> files, Guid folderId);
    }
}
