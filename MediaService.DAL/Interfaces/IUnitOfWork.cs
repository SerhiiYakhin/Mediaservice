#region usings

using System;
using System.Threading.Tasks;
using MediaService.DAL.Entities;

#endregion

namespace MediaService.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ObjectEntry, Guid> Objects { get; }

        IRepository<DirectoryEntry, Guid> Directories { get; }

        IRepository<FileEntry, Guid> Files { get; }

        IRepository<Tag, Guid> Tags { get; }

        IRepository<UserProfile, Guid> UsersProfiles { get; }

        IRepository<User, string> Users { get; }

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}