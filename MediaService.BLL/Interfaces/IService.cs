using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MediaService.BLL.Interfaces
{
    public interface IService<TDto, in TId>: IDisposable where TDto : class
    {
        TDto FindById(TId id);

        Task<TDto> FindByIdAsync(TId id);


        IEnumerable<TDto> GetData();

        Task<IEnumerable<TDto>> GetDataAsync();


        IEnumerable<TDto> GetDataParallel();

        Task<IEnumerable<TDto>> GetDataAsyncParallel();


        IdentityResult Add(TDto item);

        Task<IdentityResult> AddAsync(TDto item);


        IdentityResult AddRange(IEnumerable<TDto> items);

        Task<IdentityResult> AddRangeAsync(IEnumerable<TDto> items);


        IdentityResult AddRangeParallel(IEnumerable<TDto> items);

        Task<IdentityResult> AddRangeAsyncParallel(IEnumerable<TDto> items);


        IdentityResult Update(TDto item);

        Task<IdentityResult> UpdateAsync(TDto item);


        IdentityResult Remove(TDto item);

        Task<IdentityResult> RemoveAsync(TDto item);


        IdentityResult RemoveRange(IEnumerable<TDto> items);

        Task<IdentityResult> RemoveRangeAsync(IEnumerable<TDto> items);

        IdentityResult RemoveRangeParallel(IEnumerable<TDto> items);

        Task<IdentityResult> RemoveRangeAsyncParallel(IEnumerable<TDto> items);
    }
}
