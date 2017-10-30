using System;
using System.Threading.Tasks;
using MediaService.DAL.EF;
using MediaService.DAL.Entities;
using MediaService.DAL.Interfaces;

namespace MediaService.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _db;

        private bool _disposed = false;

        private IRepository<ObjectEntry>    _objects;

        private IRepository<DirectoryEntry> _directories;

        private IRepository<FileEntry>      _files;

        private IRepository<Tag>            _tags;

        private IRepository<UserProfile>    _users;

        public EFUnitOfWork(string connectionString)
        {
            _db = new DatabaseContext(connectionString);
        }

        #region Repositories

        public IRepository<ObjectEntry>    Objects     => _objects     ?? (_objects     = new EFRepository<ObjectEntry>(_db));

        public IRepository<DirectoryEntry> Directories => _directories ?? (_directories = new EFRepository<DirectoryEntry>(_db));

        public IRepository<FileEntry>      Files       => _files       ?? (_files       = new EFRepository<FileEntry>(_db));

        public IRepository<Tag>            Tags        => _tags        ?? (_tags        = new EFRepository<Tag>(_db));

        public IRepository<UserProfile>    Users       => _users       ?? (_users       = new EFRepository<UserProfile>(_db));

        #endregion

        public async Task SaveAsync() => await _db.SaveChangesAsync();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
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
