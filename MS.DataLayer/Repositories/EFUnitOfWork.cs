using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using MS.DataLayer.EF;
using MS.DataLayer.Identity;
using MS.DataLayer.Entities;
using MS.DataLayer.Interfaces;

namespace MS.DataLayer.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;

        private readonly string _appName;

        private bool _disposed = false;

        private ApplicationUserManager   _userManager;

        private ApplicationRoleManager   _roleManager;

        private ApplicationSignInManager _signInManager;

        private IRepository<ObjectEntry>    _objects;

        private IRepository<DirectoryEntry> _directories;

        private IRepository<FileEntry>      _files;

        private IRepository<Tag>            _tags;

        private IRepository<UserProfile>    _users;

        public EFUnitOfWork(string connectionString, string appName)
        {
            _appName = appName;
            _db = new ApplicationContext(connectionString);
        }

        #region Managers

        public ApplicationUserManager   UserManager => _userManager ?? (_userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db),
                                                           new IdentityFactoryOptions<ApplicationUserManager>
                                                           {
                                                               DataProtectionProvider =
                                                                   new Microsoft.Owin.Security.DataProtection.DpapiDataProtectionProvider(_appName)
                                                           }));

        public ApplicationRoleManager   RoleManager   => _roleManager ?? (_roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db)));

        //todo: change new OwinContext().Authentication to correct variant
        public ApplicationSignInManager SignInManager => _signInManager ?? (_signInManager = new ApplicationSignInManager(UserManager, new OwinContext().Authentication));

        #endregion

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
                    UserManager.Dispose();
                    RoleManager.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
