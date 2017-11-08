using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
namespace MediaService.BLL.Services.ObjectsServices
{
    public class FileService : ObjectsCommonService<FileEntryDto, FileEntry>, IFilesService
    {
        public FileService(IUnitOfWork uow) : base(uow)
        {
        }
    }
}
