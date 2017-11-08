using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MediaService.DAL.Interfaces
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        TEntity FindByKey(TKey key);

        Task<TEntity> FindByKeyAsync(TKey key);


        IQueryable<TEntity> GetQuery();

        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);


        IEnumerable<TEntity> GetData();

        Task<IEnumerable<TEntity>> GetDataAsync();


        IEnumerable<TEntity> GetDataParallel();

        Task<IEnumerable<TEntity>> GetDataAsyncParallel();


        IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> predicate);


        IEnumerable<TEntity> GetDataParallel(Expression<Func<TEntity, bool>> predicate);

        Task<IEnumerable<TEntity>> GetDataAsyncParallel(Expression<Func<TEntity, bool>> predicate);

        void Add(TEntity item);

        Task AddAsync(TEntity item);


        void AddRange(IEnumerable<TEntity> items);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        void AddRangeParallel(IEnumerable<TEntity> items);

        Task AddRangeAsyncParallel(IEnumerable<TEntity> items);


        void Update(TEntity item);

        Task UpdateAsync(TEntity item);


        void Remove(TEntity item);

        Task RemoveAsync(TEntity item);


        void RemoveRange(IEnumerable<TEntity> items);

        Task RemoveRangeAsync(IEnumerable<TEntity> items);

        void RemoveRangeParallel(IEnumerable<TEntity> items);

        Task RemoveRangeAsyncParallel(IEnumerable<TEntity> items);

    }
}
