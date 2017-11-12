using MediaService.DAL.EF;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;
using System;
using System.Threading.Tasks;

namespace MediaService.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;

        private bool _disposed = false;

        private IRepository<ObjectEntry, Guid>    _objects;

        private IRepository<DirectoryEntry, Guid> _directories;

        private IRepository<FileEntry, Guid>      _files;

        private IRepository<Tag, Guid>            _tags;

        private IRepository<UserProfile, Guid>  _usersProfiles;

        private IRepository<User, string> _users;

        public EFUnitOfWork(string connectionString)
        {
            _db = new DatabaseContext(connectionString);
        }

        #region Repositories

        public IRepository<ObjectEntry, Guid>    Objects     => _objects     ?? (_objects     = new EFRepository<ObjectEntry, Guid>(_db));

        public IRepository<DirectoryEntry, Guid> Directories => _directories ?? (_directories = new EFRepository<DirectoryEntry, Guid>(_db));

        public IRepository<FileEntry, Guid>      Files       => _files       ?? (_files       = new EFRepository<FileEntry, Guid>(_db));

        public IRepository<Tag, Guid>            Tags        => _tags        ?? (_tags        = new EFRepository<Tag, Guid>(_db));

        public IRepository<UserProfile, Guid>  UsersProfiles       => _usersProfiles       ?? (_usersProfiles = new EFRepository<UserProfile, Guid>(_db));

        public IRepository<User, string> Users => _users ?? (_users = new EFRepository<User, string>(_db));

        #endregion

        public int SaveChanges() => _db.SaveChanges();

        public async Task<int> SaveChangesAsync() => await _db.SaveChangesAsync();

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
    }
}
