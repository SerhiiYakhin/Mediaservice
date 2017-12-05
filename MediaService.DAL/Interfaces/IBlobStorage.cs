#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

#endregion

namespace MediaService.DAL.Interfaces
{
    public interface IBlobStorage
    {
        void Upload(Stream file, string fileName, string contentType);

        Task UploadAsync(Stream file, string fileName, string contentType);

        Task UploadAsync(byte[] file, string fileName, string contentType);

        void UploadFileInBlocks(byte[] file, string fileName, string contentType);

        Task UploadFileInBlocksAsync(byte[] file, string fileName, string contentType);

        Task UploadFileInBlocksAsync(Stream file, string fileName, string contentType);

        Task DeleteAsync(string fileName);

        Task DeleteRangeAsync(IEnumerable<string> fileNames);

        Task DeleteRangeAsync(params string[] fileNames);

        Task<(Stream blobStream, string contentType)> DownloadAsync(string blobName, int? blobSize = null);

        Task DownloadAsync(Stream blobStream, string blobName, int? blobSize = null);

        Task<bool> BlobExistAsync(string blobName);

        string GetDirectLinkToBlob(string blobName, DateTimeOffset expiryTime, SharedAccessBlobPermissions permissions);
    }
}