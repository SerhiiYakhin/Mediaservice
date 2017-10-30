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
            //modelBuilder.Entity<ObjectEntry>()
            //    .Property(e => e.Thumbnail)
            //    .IsFixedLength();

            //modelBuilder.Entity<FileEntry>()
            //    .HasMany(e => e.Tags)
            //    .WithMany(e => e.FileEntries)
            //    .Map(m => m.ToTable("FilesTags").MapLeftKey("Id").MapRightKey("Id"));

            //modelBuilder.Entity<ObjectEntry>()
            //    .HasMany(e => e.Owners)
            //    .WithMany(e => e.ObjectEntries)
            //    .Map(m => m.ToTable("UsersObjects").MapLeftKey("ObjectId").MapRightKey("UserId"));
        }
    }
}
