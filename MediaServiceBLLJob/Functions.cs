using System.IO;
using Microsoft.Azure.WebJobs;

namespace MediaServiceBLLJob
{
    public class Functions
    {
        private const string DownloadQueueName = "download";

        private const string RepositoryQueueName = "repository";

        // This function will get triggered/executed when a new message is written 
        // on an Azure Queue called queue.
        public static void ProcessDownloadQueueMessage([QueueTrigger(DownloadQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
        }

        public static void ProcessRepositoryQueueMessage([QueueTrigger(RepositoryQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
        }
    }
}
