using MediaService.DAL.Accessors.Enums;
using MediaService.DAL.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading.Tasks;

namespace MediaService.DAL.Accessors
{
    public class AzureStorageQueueAccessor : IQueueStorage
    {
        #region Fields

        private const string DownloadQueueName = "download";

        private const string RepositoryQueueName = "repository";

        private static string _connectionString;

        private readonly TimeSpan _timeToLive = new TimeSpan(0, 1, 0);

        #endregion

        #region Constructors

        public AzureStorageQueueAccessor(string connectionString)
        {
            _connectionString = connectionString;
        }

        #endregion

        #region Methods

        public void AddMessage(string messageContent, QueueJob queueJob)
        {
            var queue = GetQueue(queueJob);
            var message = new CloudQueueMessage(messageContent);

            queue.AddMessage(message, _timeToLive);
        }

        public async Task AddMessageAsync(string messageContent, QueueJob queueJob)
        {
            var queue = GetQueue(queueJob);
            var message = new CloudQueueMessage(messageContent);

            await queue.AddMessageAsync(message, _timeToLive, null, null, null);
        }

        #endregion

        #region Help Methods

        private static CloudQueue GetQueue(QueueJob queueJob)
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue;

            switch (queueJob)
            {
                case QueueJob.Download:
                    queue = queueClient.GetQueueReference(DownloadQueueName);
                    break;
                case QueueJob.DataOperation:
                    queue = queueClient.GetQueueReference(RepositoryQueueName);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(queueJob), queueJob, "There is no such queue to work with");
            }

            queue.CreateIfNotExists();

            return queue;
        }

        #endregion
    }
}
