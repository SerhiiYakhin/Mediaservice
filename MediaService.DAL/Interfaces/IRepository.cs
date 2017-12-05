#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace MediaService.DAL.Interfaces
{
    public interface IRepository<TEntity, in TKey> where TEntity : class
    {
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);


        TEntity FindByKey(TKey key);

        Task<TEntity> FindByKeyAsync(TKey key);


        IQueryable<TEntity> GetQuery();

        IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetData();


        void Add(TEntity item);

        Task AddAsync(TEntity item);


        void AddRange(IEnumerable<TEntity> items);

        Task AddRangeAsync(IEnumerable<TEntity> items);

        void Update(TEntity item);

        Task UpdateAsync(TEntity item);


        void Remove(TEntity item);

        Task RemoveAsync(TEntity item);


        void RemoveRange(IEnumerable<TEntity> items);

        Task RemoveRangeAsync(IEnumerable<TEntity> items);
    }
}