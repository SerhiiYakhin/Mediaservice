using System.Data.Entity;
using MediaService.DAL.Entities;

namespace MediaService.DAL.EF
{
    class DatabaseContext : DbContext
    {
        public virtual DbSet<ObjectEntry>    ObjectEntries    { get; set; }

        public virtual DbSet<FileEntry>      FileEntries      { get; set; }

        public virtual DbSet<DirectoryEntry> DirectoryEntries { get; set; }

        public virtual DbSet<Tag>            Tags             { get; set; }

        public virtual DbSet<UserProfile>    UserProfiles     { get; set; }

        public DatabaseContext(string connectionString) : base(connectionString)
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
