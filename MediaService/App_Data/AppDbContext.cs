using System.Data.Entity;
using MediaService.Models;
using MediaService.Models.AppModels;
using MediaService.Models.AspNetModels;

namespace MediaService.App_Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("name=DataEntities")
        {
        }

        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Object> Objects { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<Object>()
                .Property(e => e.Thumbnail)
                .IsFixedLength();

            modelBuilder.Entity<Object>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Objects)
                .Map(m => m.ToTable("ObjectsTags").MapLeftKey("ObjectId").MapRightKey("TagId"));

            modelBuilder.Entity<Object>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.Objects)
                .Map(m => m.ToTable("UsersObjects").MapLeftKey("ObjectId").MapRightKey("UserId"));
        }
    }
}
