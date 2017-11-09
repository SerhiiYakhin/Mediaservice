using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class DirectoryService : ObjectsCommonService<DirectoryEntryDto, DirectoryEntry>, IDirectoryService
    {
        public DirectoryService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Directories;
        }
    }
}
