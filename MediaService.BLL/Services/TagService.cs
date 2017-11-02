using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
    public class TagService : Service<TagDto, Guid>, ITagService
    {
        public TagService(IUnitOfWork uow) : base(uow)
        {
            Repository = Database.Tags;

            EntityType = typeof(Tag);

            CollectionEntityType = typeof(IEnumerable<Tag>);
        }

        public TagDto GetTagByName(string name)
        {
            return DtoMapper.Map<TagDto>(Database.Tags.GetData(t => t.Name.Equals(name)).SingleOrDefault());
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            return DtoMapper.Map<TagDto>((await Database.Tags.GetDataAsync(t => t.Name.Equals(name)))
                .SingleOrDefault());
        }
    }
}
