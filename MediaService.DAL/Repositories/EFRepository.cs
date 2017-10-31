using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediaService.DAL.Interfaces;

namespace MediaService.DAL.Repositories
{
    public class EFRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext      _context;
        private readonly DbSet<TEntity> _dbSet;

        public EFRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get() => _dbSet.AsNoTracking().ToList();

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.AsNoTracking().Where(predicate).ToList();
        }

        public Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate)
        {

            return _dbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public TEntity FindById(int id) => _dbSet.Find(id);

        public void Create(TEntity item) => _dbSet.Add(item);

        public void Update(TEntity item) => _context.Entry(item).State = EntityState.Modified;

        public void Remove(TEntity item) => _dbSet.Remove(item);
    }
}
