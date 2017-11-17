using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MediaService.DAL.Interfaces;

namespace MediaService.DAL.Accessors
{
    public class EFRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
    {
        private readonly DbContext      _context;
        private readonly DbSet<TEntity> _dbSet;

        public EFRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public TEntity FindByKey(TKey key) => _dbSet.Find(key);

        public async Task<TEntity> FindByKeyAsync(TKey key) => await _dbSet.FindAsync(key);


        public IQueryable<TEntity> GetQuery()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }


        public IEnumerable<TEntity> GetData() => _dbSet;

        public async Task<IEnumerable<TEntity>> GetDataAsync() => await Task.Run(() => _dbSet);


        public IEnumerable<TEntity> GetData(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetDataAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => _dbSet.Where(predicate));
        }


        public void Add(TEntity item) => _dbSet.Add(item);

        public async Task AddAsync(TEntity item) => await Task.Run(() => _dbSet.Add(item));


        public void AddRange(IEnumerable<TEntity> items) => _dbSet.AddRange(items);

        public async Task AddRangeAsync(IEnumerable<TEntity> items) => await Task.Run(() => _dbSet.AddRange(items));


        public void Update(TEntity item) => _context.Entry(item).State = EntityState.Modified;

        public async Task UpdateAsync(TEntity item) => await Task.Run(() => _context.Entry(item).State = EntityState.Modified);


        public void Remove(TEntity item) => _dbSet.Remove(item);

        public async Task RemoveAsync(TEntity item) => await Task.Run(() => _dbSet.Remove(item));


        public void RemoveRange(IEnumerable<TEntity> items) => _dbSet.RemoveRange(items);

        public async Task RemoveRangeAsync(IEnumerable<TEntity> items) => await Task.Run(() => _dbSet.RemoveRange(items));
    }
}
