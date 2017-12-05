#region usings

using MediaService.BLL.DTO;
using System;
using System.IO;
using System.Threading.Tasks;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IDirectoryService : IObjectService<DirectoryEntryDto>
    {
        Task<DirectoryEntryDto> GetRootAsync(string ownerId);

        void AddRootDirToUser(string userId);

        Task AddRootDirToUserAsync(string userId);

        void Rename(DirectoryEntryDto editedDirEntryDto);

        Task RenameAsync(DirectoryEntryDto editedDirEntryDto);

        Task DeleteAsync(Guid entryId);

        Task DeleteWithJobAsync(Guid entryId);

        Task DownloadWithJobAsync(Guid directoryId, Guid zipId);

        Task DownloadAsync(Guid directoryId, Guid zipId);

        Task<(Stream blobStream, string blobContentType)> DownloadZip(string zipName);
    }
}