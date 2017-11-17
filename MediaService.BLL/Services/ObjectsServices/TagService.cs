using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services.ObjectsServices
{
    public class TagService : Service<TagDto, Tag, Guid>,  ITagService
    {
        public TagService(IUnitOfWork uow) : base(uow)
        {
            Repository = uow.Tags;
        }

        public TagDto GetTagByName(string name)
        {
            return DtoMapper.Map<TagDto>(Context.Tags.GetData(t => t.Name.Equals(name)).SingleOrDefault());
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            return DtoMapper.Map<TagDto>((await Context.Tags.GetDataAsync(t => t.Name.Equals(name)))
                .SingleOrDefault());
        }
    }
}
