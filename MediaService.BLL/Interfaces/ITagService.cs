using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;

namespace MediaService.BLL.Interfaces
{
    public interface ITagService : IDisposable
    {
        TagDto GetTag(Guid? id);
        IEnumerable<TagDto> GetTags();
        OperationDetails CreateTags(params TagDto[] list);
        OperationDetails DeleteTags(params TagDto[] list);

        Task<TagDto> GetTagAsync(Guid? id);
        Task<IEnumerable<TagDto>> GetTagsAsync();
        Task<OperationDetails> CreateTagsAsync(params TagDto[] list);
        Task<OperationDetails> DeleteTagsAsync(params TagDto[] list);
    }
}
