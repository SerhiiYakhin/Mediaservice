using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using MS.DataLayer.Entities;

namespace MS.DataLayer.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<ObjectEntry> ObjectEntries { get; set; }
        public virtual DbSet<FileEntry> FileEntries { get; set; }
        public virtual DbSet<DirectoryEntry> DirectoryEntries { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}
