#region usings

using System.IO;
using System.Threading.Tasks;

#endregion

namespace MediaService.DAL.Interfaces
{
    public interface IStorage
    {
        void Upload(Stream file, string fileName);

        Task UploadAsync(Stream file, string fileName);

        Task UploadAsync(byte[] file, string fileName);

        void UploadFileInBlocks(byte[] file, string fileName);

        Task UploadFileInBlocksAsync(byte[] file, string fileName);

        Task UploadFileInBlocksAsync(Stream file, string fileName);
    }
}