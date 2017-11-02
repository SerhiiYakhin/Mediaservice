using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaService.BLL.Infrastructure;
using Microsoft.AspNet.Identity;

namespace MediaService.BLL.Interfaces
{
    public interface IService<TDto, in TId>: IDisposable where TDto : class
    {
        TDto FindById(TId id);

        Task<TDto> FindByIdAsync(TId id);


        IEnumerable<TDto> GetData();

        Task<IEnumerable<TDto>> GetDataAsync();


        IdentityResult Add(TDto item);

        Task<IdentityResult> AddAsync(TDto item);


        IdentityResult AddRange(IEnumerable<TDto> items);

        Task<IdentityResult> AddRangeAsync(IEnumerable<TDto> items);


        IdentityResult Update(TDto item);

        Task<IdentityResult> UpdateAsync(TDto item);


        IdentityResult Remove(TDto item);

        Task<IdentityResult> RemoveAsync(TDto item);


        IdentityResult RemoveRange(IEnumerable<TDto> items);

        Task<IdentityResult> RemoveRangeAsync(IEnumerable<TDto> items);
    }
}
