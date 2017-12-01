#region usings

using MediaService.DAL.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

#endregion

namespace MediaService.DAL.Accessors
{
    public class AzureStorageBlobAccessor : IBlobStorage
    {
        #region Fields

        private const string ContainerName = "files";

        private const string ImagesContainerName = "images";

        private const string VideosContainerName = "videos";

        private const int MaxAttempts = 1;

        private const int MaxBlocksCount = 50000;

        //Value: The size of a block, in bytes, ranging from between 16 KB and 100 MB inclusive.
        private const int MaxBlockSize = 4_194_304;

        //If blobs are small(less than 256 MB), keeping this value equal to 1 is advised.
        private const int BytesToAddThread = 268_435_456;

        private static string _connectionString;

        #endregion

        #region Constructors

        public AzureStorageBlobAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Methods

        #region Upload

        public void Upload(Stream file, string fileName, string contentType)
        {
            var blob = PrepapreBlobToUpload(fileName, contentType);
            blob.UploadFromStream(file);
        }

        public async Task UploadAsync(Stream file, string fileName, string contentType)
        {
            var blob = PrepapreBlobToUpload(fileName, contentType);
            await blob.UploadFromStreamAsync(file);
        }

        public async Task UploadAsync(byte[] file, string fileName, string contentType)
        {
            var blob = PrepapreBlobToUpload(fileName, contentType);
            await blob.UploadFromByteArrayAsync(file, 0, file.Length);
        }

        public void UploadFileInBlocks(byte[] file, string fileName, string contentType)
        {
            PrepareBlobToUploadInBlocks(file.Length, fileName, contentType, out var blob, out var requestOptions);
            blob.UploadFromByteArray(file, 0, file.Length, null, requestOptions);
        }

        public async Task UploadFileInBlocksAsync(byte[] file, string fileName, string contentType)
        {
            PrepareBlobToUploadInBlocks(file.Length, fileName, contentType, out var blob, out var requestOptions);
            await blob.UploadFromByteArrayAsync(file, 0, file.Length, null, requestOptions, null);
        }

        public async Task UploadFileInBlocksAsync(Stream file, string fileName, string contentType)
        {
            PrepareBlobToUploadInBlocks(file.Length, fileName, contentType, out var blob, out var requestOptions);
            await blob.UploadFromStreamAsync(file, null, requestOptions, null);
        }

        #endregion

        #region Download

        public async Task<(Stream blobStream, string contentType)> DownloadAsync(string blobName, int? blobSize = null)
        {
            var blob = GetBlob(blobName);
            var blobExist = await blob.ExistsAsync();

            if (blobExist)
            {
                var blobStream = new MemoryStream();
                await LoadBlobToStream(blobStream, blob, blobSize);
                return (blobStream, blob.Properties.ContentType);
            }

            return (null, null);
        }

        public async Task DownloadAsync(Stream blobStream, string blobName, int? blobSize = null)
        {
            var blob = GetBlob(blobName);
            await LoadBlobToStream(blobStream, blob, blobSize);
        }

        public async Task<bool> BlobExistAsync(string blobName)
        {
            return await GetContainerReference()
                        .GetBlockBlobReference(blobName)
                        .ExistsAsync();
        }

        public string GetDirectLinkToBlob(string blobName, DateTimeOffset expiryTime, SharedAccessBlobPermissions permissions)
        {
            var blob = GetBlob(blobName);

            if (blob.Exists())
            {    
                var sasConstraints = new SharedAccessBlobPolicy
                {
                    SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-5),
                    SharedAccessExpiryTime = expiryTime,
                    Permissions = permissions
                };

                var sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

                return blob.Uri + sasBlobToken;
            }

            return null;
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(string fileName)
        {
            var blob = GetBlob(fileName);
            await blob.DeleteAsync();
        }

        #endregion

        #endregion

        #region Help Methods

        private static async Task LoadBlobToStream(Stream blobStream, CloudBlockBlob blob, int? blobSize)
        {
            int threadCount = 1;

            if (blobSize.HasValue && blobSize.Value > BytesToAddThread)
            {
               threadCount = blobSize.Value / BytesToAddThread + 1;
            }

            var requestOptions = new BlobRequestOptions
            {
                SingleBlobUploadThresholdInBytes = (long)(blob.StreamWriteSizeInBytes * 1.6),
                ParallelOperationThreadCount = threadCount,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), MaxAttempts)
            };

            await blob.DownloadToStreamAsync(blobStream, null, requestOptions, null);
        }

        private static CloudBlockBlob PrepapreBlobToUpload(string fileName, string contentType)
        {
            var blob = GetBlob(fileName);
            blob.Properties.ContentType = contentType;
            blob.DeleteIfExists();

            return blob;
        }

        private static void PrepareBlobToUploadInBlocks(long fileLenght, string fileName, string contentType, out CloudBlockBlob blob, out BlobRequestOptions requestOptions)
        {
            blob = PrepapreBlobToUpload(fileName, contentType);
            blob.StreamWriteSizeInBytes = GetBlockSize(fileLenght);

            var threadCount = (int)Math.Ceiling((double)fileLenght / BytesToAddThread);

            requestOptions = new BlobRequestOptions
            {
                SingleBlobUploadThresholdInBytes = (long)(blob.StreamWriteSizeInBytes * 1.6),
                ParallelOperationThreadCount = threadCount,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), MaxAttempts)
            };
        }

        private static int GetBlockSize(long fileSize)
        {
            long blocksize = 1_024_000;
            long blockCount = GetBlockCount(fileSize, blocksize);

            while (blockCount > MaxBlocksCount - 1)
            {
                blocksize += 1_024_000;
                blockCount = GetBlockCount(fileSize, blocksize);
            }

            if (blocksize > MaxBlockSize)
            {
                throw new ArgumentException("Blob too big to upload.");
            }

            return (int)blocksize;
        }

        private static long GetBlockCount(long fileSize, long blocksize)
        {
            return (int)Math.Ceiling((double)fileSize / blocksize);
        }

        private static string GetBase64BlockId(int blockId)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes($"{blockId:0000000}"));
        }

        private static CloudBlockBlob GetBlob(string blobName)
        {
            var container = GetContainerReference();
            var blob = container.GetBlockBlobReference(blobName);

            return blob;
        }

        private static CloudBlobContainer GetContainerReference()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);
            container.SetPermissionsAsync(
                new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Off
                }
                );
            container.CreateIfNotExists();

            return container;
        }

        #endregion
    }
}