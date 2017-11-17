#region usings

using System;
using System.Threading.Tasks;
using MediaService.BLL.DTO;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface ITagService : IService<TagDto, Guid>
    {
        TagDto GetTagByName(string name);

        Task<TagDto> GetTagByNameAsync(string name);
    }
}