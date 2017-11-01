using System;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

namespace MediaService.BLL.Interfaces
{
    public interface ITagService : IService<TagDto, Guid>
    {
        TagDto GetTagByName(string name);

        Task<TagDto> GetTagByNameAsync(string name);
    }
}