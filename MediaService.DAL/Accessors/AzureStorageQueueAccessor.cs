using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace MediaService.DAL.Accessors
{
    class AzureStorageQueueAccessor
    {
        #region Constructors

        public AzureStorageQueueAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Fields

        private const string ContainerName = "files";

        private const string ImagesContainerName = "images";

        private const string VideosContainerName = "videos";

        private const string DownloadQueueName = "download";
        private const string ThumbnailQueueName = "thumbnail";
        private const string RepositoryQueueName = "repository";

        //2 * 1024 * 1024 bytes or 2 MB
        private const int BlockSize = 2_097_152;

        private static string _connectionString;

        #endregion

        #region Methods

        public void AddMessage(string messageContent)
        {
            var queue = GetQueue(DownloadQueueName);
            var message = new CloudQueueMessage(messageContent);

            queue.AddMessage(message, new TimeSpan(0, 1, 0));
        }

        #endregion

        #region Help Methods

        private static CloudQueue GetQueue(string queueName)
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();

            var queue = queueClient.GetQueueReference(queueName);
            queue.CreateIfNotExists();
            return queue;
        }

        #endregion
    }
}
