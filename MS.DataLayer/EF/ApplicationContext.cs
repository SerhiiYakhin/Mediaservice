using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.DataLayer.Entities;

namespace MS.DataLayer.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ObjectEntry>    ObjectEntries    { get; set; }
        public virtual DbSet<FileEntry>      FileEntries      { get; set; }
        public virtual DbSet<DirectoryEntry> DirectoryEntries { get; set; }
        public virtual DbSet<Tag>            Tags             { get; set; }
        public virtual DbSet<UserProfile>    UserProfiles     { get; set; }

        public ApplicationContext() : this("DefaultConnection") { }

        public ApplicationContext(string connectionString)
            : base(connectionString, throwIfV1Schema: false)
        {
        }

        public static ApplicationContext Create() => new ApplicationContext();
    }
}
