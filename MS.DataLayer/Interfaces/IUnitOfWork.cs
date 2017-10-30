using System;
using System.Threading.Tasks;
using MS.DataLayer.Entities;
using MS.DataLayer.Identity;

namespace MS.DataLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<ObjectEntry>    Objects     { get; }

        IRepository<DirectoryEntry> Directories { get; }

        IRepository<FileEntry>      Files       { get; }

        IRepository<Tag>            Tags        { get; }

        IRepository<UserProfile>    Users       { get; }


        ApplicationUserManager UserManager   { get; }

        ApplicationRoleManager RoleManager   { get; }

        Task SaveAsync();
    } 
}
