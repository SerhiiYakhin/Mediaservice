#region usings

using System;
using System.IO;
using System.Threading.Tasks;
using MediaService.DAL.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.RetryPolicies;

#endregion

namespace MediaService.DAL.Accessors
{
    public class AzureStorageAccessor : IStorage
    {
        #region Constructors

        public AzureStorageAccessor(string connectionString)
        {
        }

        #endregion

        #region Fields

        private const string ConnectionStringSettingName = "StorageConnectionString";

        private const string ContainerName = "files";

        private const string ImagesContainerName = "images";

        private const string VideosContainerName = "videos";

        //2 * 1024 * 1024 bytes or 2 MB
        private const int BlockSize = 2_097_152;

        private static readonly string ConnectionString = "UseDevelopmentStorage=true;";
        //private static readonly string ConnectionString = CloudConfigurationManager.GetSetting(ConnectionStringSettingName);

        #endregion

        #region Methods

        public void Upload(Stream file, string fileName)
        {
            var blob = GetBlob(fileName);
            blob.UploadFromStream(file);
        }

        public async Task UploadAsync(Stream file, string fileName)
        {
            var blob = GetBlob(fileName);
            await blob.UploadFromStreamAsync(file);
        }

        public async Task UploadAsync(byte[] file, string fileName)
        {
            var blob = GetBlob(fileName);
            await blob.UploadFromByteArrayAsync(file, 0, file.Length);
        }

        public void UploadFileInBlocks(byte[] file, string fileName)
        {
            var blob = GetBlob(fileName);

            var requestOptions = new BlobRequestOptions
            {
                ParallelOperationThreadCount = 2,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 1)
            };

            blob.StreamWriteSizeInBytes = BlockSize;

            blob.UploadFromByteArray(file, 0, file.Length, null, requestOptions);
        }

        //todo: Check if it works
        public async Task UploadFileInBlocksAsync(byte[] file, string fileName)
        {
            var blob = GetBlob(fileName);

            var requestOptions = new BlobRequestOptions
            {
                ParallelOperationThreadCount = 2,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 1)
            };

            blob.StreamWriteSizeInBytes = BlockSize;

            await blob.UploadFromByteArrayAsync(file, 0, file.Length, null, requestOptions, null);
        }

        //todo: Check if it works
        public async Task UploadFileInBlocksAsync(Stream file, string fileName)
        {
            var blob = GetBlob(fileName);

            var requestOptions = new BlobRequestOptions
            {
                ParallelOperationThreadCount = 2,
                RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(2), 1)
            };

            blob.StreamWriteSizeInBytes = BlockSize;

            await blob.UploadFromStreamAsync(file, null, requestOptions, null);
        }

        #endregion

        #region Help Methods

        private static CloudBlockBlob GetBlob(string fileName)
        {
            var container = GetContainerReference();
            var blob = container.GetBlockBlobReference(fileName);
            return blob;
        }

        private static CloudBlobContainer GetContainerReference()
        {
            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(ContainerName);

            container.CreateIfNotExists();

            return container;
        }

        #endregion
    }
}