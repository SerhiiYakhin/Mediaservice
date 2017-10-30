using System;
using System.Collections.Generic;

namespace MediaService.DAL.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity item);

        TEntity FindById(int id);

        IEnumerable<TEntity> Get();

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        void Remove(TEntity item);

        void Update(TEntity item);
    }
}
