using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class ObjectService : ObjectsCommonService<ObjectEntryDto, ObjectEntry>, IObjectService<ObjectEntryDto>
    {
        public ObjectService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Objects;
        }
    }
}
