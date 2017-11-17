#region usings

using System;
using System.Threading.Tasks;
using MediaService.DAL.EF;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

#endregion

namespace MediaService.DAL.Accessors
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;

        private IRepository<DirectoryEntry, Guid> _directories;

        private bool _disposed;

        private IRepository<FileEntry, Guid> _files;

        private IRepository<ObjectEntry, Guid> _objects;

        private IRepository<Tag, Guid> _tags;

        private IRepository<User, string> _users;

        private IRepository<UserProfile, Guid> _usersProfiles;

        public EFUnitOfWork(string connectionString)
        {
            _db = new DatabaseContext(connectionString);
        }

        public int SaveChanges()
        {
            return _db.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                _disposed = true;
            }
        }

        #region Repositories

        public IRepository<ObjectEntry, Guid> Objects =>
            _objects ?? (_objects = new EFRepository<ObjectEntry, Guid>(_db));

        public IRepository<DirectoryEntry, Guid> Directories =>
            _directories ?? (_directories = new EFRepository<DirectoryEntry, Guid>(_db));

        public IRepository<FileEntry, Guid> Files => _files ?? (_files = new EFRepository<FileEntry, Guid>(_db));

        public IRepository<Tag, Guid> Tags => _tags ?? (_tags = new EFRepository<Tag, Guid>(_db));

        public IRepository<UserProfile, Guid> UsersProfiles =>
            _usersProfiles ?? (_usersProfiles = new EFRepository<UserProfile, Guid>(_db));

        public IRepository<User, string> Users => _users ?? (_users = new EFRepository<User, string>(_db));

        #endregion
    }
}