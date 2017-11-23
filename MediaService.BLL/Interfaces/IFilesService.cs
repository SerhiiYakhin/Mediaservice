#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IFilesService : IObjectService<FileEntryDto>
    {
        Task AddRangeAsync(IEnumerable<FileEntryDto> files, Guid parentId);

        Task RenameAsync(FileEntryDto editedDirEntryDto);

        Task DeleteAsync(Guid entryId);

        Task DownloadWithJobAsync(IEnumerable<Guid> filesIds, Guid zipId);

        Task DownloadAsync(IEnumerable<Guid> filesIds, Guid zipId);

        Task<string> GetLinkToFileAsync(string fileName);

        Task<string> GetLinkToFileAsync(Guid fileId);

        Task<Stream> GetFileThumbnailAsync(Guid fileId);

        Task AddTagAsync(Guid fileId, string tagName);
    }
}