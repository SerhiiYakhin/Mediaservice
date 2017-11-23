using System.Threading.Tasks;
using MediaService.DAL.Accessors.Enums;

namespace MediaService.DAL.Interfaces
{
    public interface IQueueStorage
    {
        void AddMessage(string messageContent, QueueJob queueJob);

        Task AddMessageAsync(string messageContent, QueueJob queueJob);
    }
}
