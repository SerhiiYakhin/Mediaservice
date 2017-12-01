#region usings

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace MediaService.BLL.Interfaces
{
    public interface IService<TDto, in TId> : IDisposable where TDto : class
    {
        TDto FindById(TId id);

        Task<TDto> FindByIdAsync(TId id);

        IEnumerable<TDto> GetData();

        Task<IEnumerable<TDto>> GetDataAsync();

        void Add(TDto item);

        Task AddAsync(TDto item);
    }
}