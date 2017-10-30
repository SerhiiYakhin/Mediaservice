using System;
using System.Threading.Tasks;
using MediaService.DAL.Entities;

namespace MediaService.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ObjectEntry>    Objects     { get; }

        IRepository<DirectoryEntry> Directories { get; }

        IRepository<FileEntry>      Files       { get; }

        IRepository<Tag>            Tags        { get; }

        IRepository<UserProfile>    Users       { get; }

        Task SaveAsync();
    } 
}
