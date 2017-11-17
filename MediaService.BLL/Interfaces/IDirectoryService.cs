#region usings

using System;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IDirectoryService : IObjectService<DirectoryEntryDto>
    {
        Task<bool> IsDirectoryExistAsync(string name, Guid parentId);

        Task AddRootDirToUserAsync(string userId);

        Task<DirectoryEntryDto> GetRootAsync(string ownerId);

        Task AddAsync(string name, Guid parentId);
    }
}