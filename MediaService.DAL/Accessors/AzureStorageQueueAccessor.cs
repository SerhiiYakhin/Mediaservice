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

        private static readonly string[] _queueNames = new string[] { "download", "thumbnails", "delete", "create", "update" };

        private static string _connectionString;

        private readonly TimeSpan _timeToLive = new TimeSpan(0, 2, 30);

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

            queue = queueClient.GetQueueReference(_queueNames[(int)queueJob]);

            queue.CreateIfNotExists();

            return queue;
        }

        #endregion
    }
}
