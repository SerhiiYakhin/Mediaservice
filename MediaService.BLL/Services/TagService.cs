using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
    public class TagService : Service<TagDto, Tag, Guid>,  ITagService
    {
        public TagService(IUnitOfWork uow) : base(uow) { }

        public TagDto GetTagByName(string name)
        {
            return DtoMapper.Map<TagDto>(Database.Tags.GetDataParallel(t => t.Name.Equals(name)).SingleOrDefault());
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            return DtoMapper.Map<TagDto>((await Database.Tags.GetDataAsyncParallel(t => t.Name.Equals(name)))
                .SingleOrDefault());
        }
    }
}
