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
            try
            {
                return DtoMapper.Map<TagDto>(Database.Tags.GetDataParallel(t => t.Name.Equals(name)).SingleOrDefault());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }

        public async Task<TagDto> GetTagByNameAsync(string name)
        {
            try
            {
                return DtoMapper.Map<TagDto>((await Database.Tags.GetDataAsyncParallel(t => t.Name.Equals(name)))
                    .SingleOrDefault());
            }
            catch (Exception e)
            {
                Logger.Error(e);
                return null;
            }
        }
    }
}
