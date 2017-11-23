using System;
using Microsoft.Azure.WebJobs;

namespace MediaServiceBLLJob
{
    class Program
    {
        static void Main()
        {
            var startup = new Startup();
            var config = new JobHostConfiguration();

            config.Queues.BatchSize = 8;
            config.Queues.MaxDequeueCount = 2;
            config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(15);

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var host = new JobHost(config);
            // The following code ensures that the WebJob will be running continuously
            host.RunAndBlock();
        }
    }
}
