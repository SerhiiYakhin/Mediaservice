using MediaService.BLL.Interfaces;
using MediaService.BLL.Models.Enums;
using MediaService.BLL.Models.QueueMessages;
using MediaService_BLL_Job.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MediaService_BLL_Job
{
    /// <summary>
    /// This is a listener functions
    /// </summary>
    public class Functions : FunctionBase
    {
        #region Fields

        private const string DownloadQueueName = "download";

        private const string ThumbnailsQueueName = "thumbnails";

        private const string DeleteQueueName = "delete";

        private const string CreateQueueName = "create";

        private const string UpdateQueueName = "update";

        /// <summary>
        /// Dependency injected <see cref="IFileService"/> that provides methods for interacting with user files.
        /// </summary>
        private readonly IFileService _fileService;

        /// <summary>
        /// Dependency injected <see cref="IFileService"/> that provides methods for interacting with user files.
        /// </summary>
        private readonly IDirectoryService _directoryService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of <see cref="Functions"/>.
        /// </summary>
        /// <param name="fileService">Dependency injected <see cref="IFileService"/> that provides methods for interacting with user files.</param>
        /// <param name="directoryService">Dependency injected <see cref="IDirectoryService"/> that provides methods for interacting with user folders.</param>
        public Functions(IFileService fileService, IDirectoryService directoryService) : base()
        {
            _fileService = fileService;
            _directoryService = directoryService;
        }

        #endregion

        #region Listeners

        /// <summary>
        /// Listen download queue.
        /// </summary>
        /// <param name="message">Incoming message to process.</param>
        /// <param name="log">A logging binder.</param>
        public async Task ProcessDownloadQueueMessage([QueueTrigger(DownloadQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
            try
            {
                var messageInfo = JsonConvert.DeserializeObject<DownloadMessageInfo>(message);

                switch (messageInfo.OperationType)
                {
                    case OperationType.DownloadFiles:
                        await _fileService.DownloadAsync(messageInfo.EntriesIds, messageInfo.ZipId);
                        break;
                    case OperationType.DownloadFolder:
                        await _directoryService.DownloadAsync(messageInfo.EntriesIds[0], messageInfo.ZipId);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                log.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Listen thumbnail queue.
        /// </summary>
        /// <param name="message">Incoming message to process.</param>
        /// <param name="log">A logging binder.</param>
        public async Task ProcessThumbnailsQueueMessage([QueueTrigger(ThumbnailsQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
            try
            {
                var messageInfo = JsonConvert.DeserializeObject<ThumbnailMessageInfo>(message);

                switch (messageInfo.OperationType)
                {
                    case OperationType.GenerateThumbnail:
                        await _fileService.GenerateThumbnailsToFilesAsync(messageInfo.FilesNames);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                log.WriteLine(message);
            }
        }

        /// <summary>
        /// Listen delete queue.
        /// </summary>
        /// <param name="message">Incoming message to process.</param>
        /// <param name="log">A logging binder.</param>
        public async Task ProcessDeleteQueueMessage([QueueTrigger(DeleteQueueName)] string message, TextWriter log)
        {
            try
            {
                var messageInfo = JsonConvert.DeserializeObject<DeleteMessageInfo>(message);

                switch (messageInfo.OperationType)
                {
                    case OperationType.DeleteFolder:
                        await _directoryService.DeleteAsync(messageInfo.EntryId);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                log.WriteLine(message);
            }
        }

        /// <summary>
        /// Listen create queue.
        /// </summary>
        /// <param name="message">Incoming message to process.</param>
        /// <param name="log">A logging binder.</param>
        public async Task ProcessCreateQueueMessage([QueueTrigger(CreateQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
        }

        /// <summary>
        /// Listen update queue.
        /// </summary>
        /// <param name="message">Incoming message to process.</param>
        /// <param name="log">A logging binder.</param>
        public async Task ProcessUpdateQueueMessage([QueueTrigger(UpdateQueueName)] string message, TextWriter log)
        {
            log.WriteLine(message);
        }

        #endregion

        #region Overrided Methods

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_directoryService != null)
                {
                    _directoryService.Dispose();
                }

                if (_fileService != null)
                {
                    _fileService.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
