using MediaService.BLL.DTO;
using MediaService.BLL.Interfaces;
using MediaService.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaService.BLL.Services
{
    public class TagService : ITagService
    {
        private IUnitOfWork Database { get; }

        public TagService(IUnitOfWork uow) => Database = uow;

        public void Dispose() => Database.Dispose();

        public TagDto FindById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<TagDto> FindByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TagDto> GetData()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagDto>> GetDataAsync()
        {
            throw new NotImplementedException();
        }

        public void Add(TagDto item)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(TagDto item)
        {
            throw new NotImplementedException();
        }

        public void AddRange(IEnumerable<TagDto> items)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<TagDto> items)
        {
            throw new NotImplementedException();
        }

        public void Update(TagDto item)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TagDto item)
        {
            throw new NotImplementedException();
        }

        public void Remove(TagDto item)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(TagDto item)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<TagDto> items)
        {
            throw new NotImplementedException();
        }

        public Task RemoveRangeAsync(IEnumerable<TagDto> items)
        {
            throw new NotImplementedException();
        }

        public TagDto GetTagByName(string name)
        {
            throw new NotImplementedException();
        }

        public Task<TagDto> GetTagByNameAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}
