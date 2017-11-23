#region usings

using System;
using System.IO;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IDirectoryService : IObjectService<DirectoryEntryDto>
    {
        Task AddRootDirToUserAsync(string userId);

        Task<DirectoryEntryDto> GetRootAsync(string ownerId);

        Task RenameAsync(DirectoryEntryDto editedDirEntryDto);

        Task DeleteAsync(Guid entryId);

        Task DeleteWithJobAsync(Guid entryId);

        Task DownloadWithJobAsync(Guid directoryId, Guid zipId);

        Task DownloadAsync(Guid directoryId, Guid zipId);

        Task<(Stream blobStream, bool blobExist)> DownloadZip(string zipName);
    }
}