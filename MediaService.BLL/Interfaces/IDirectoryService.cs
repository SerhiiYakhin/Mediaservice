using System;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

namespace MediaService.BLL.Interfaces
{
    public interface IDirectoryService : IObjectService<DirectoryEntryDto>
    {
        Task AddRootDirToUserAsync(string userId);

        Task<DirectoryEntryDto> GetRootAsync(string ownerId);

        Task AddAsync(string name, Guid parentId);
    }
}
