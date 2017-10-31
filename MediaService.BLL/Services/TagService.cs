using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.DTO;
using MediaService.BLL.Infrastructure;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;

namespace MediaService.BLL.Services
{
    public class TagService : ITagService
    {
        private IUnitOfWork Database { get; }

        public TagService(IUnitOfWork uow) => Database = uow;

        public TagDto GetTag(Guid? id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TagDto> GetTags()
        {
            throw new NotImplementedException();
        }

        public OperationDetails CreateTags(params TagDto[] list)
        {
            throw new NotImplementedException();
        }

        public OperationDetails DeleteTags(params TagDto[] list)
        {
            throw new NotImplementedException();
        }

        public Task<TagDto> GetTagAsync(Guid? id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagDto>> GetTagsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> CreateTagsAsync(params TagDto[] list)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> DeleteTagsAsync(params TagDto[] list)
        {
            throw new NotImplementedException();
        }

        public void Dispose() => Database.Dispose();
    }
}
