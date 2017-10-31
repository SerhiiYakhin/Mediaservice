using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MediaService.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);

        TEntity FindById(int id);

        IEnumerable<TEntity> Get();

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);

        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        void Remove(TEntity item);

        void Update(TEntity item);
    }
}
