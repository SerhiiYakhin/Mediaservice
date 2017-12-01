﻿#region usings

using System.Data.Entity;
using MediaService.DAL.Entities;

#endregion

namespace MediaService.DAL.EF
{
    internal class DatabaseContext : DbContext
    {
        #region Constructors

        public DatabaseContext(string connectionString) : base(connectionString)
        {
        }

        public DatabaseContext() : base("DefaultConnection")
        {
        }

        #endregion

        #region DbSets

        public virtual DbSet<ObjectEntry> ObjectEntries { get; set; }

        public virtual DbSet<FileEntry> FileEntries { get; set; }

        public virtual DbSet<DirectoryEntry> DirectoryEntries { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        public virtual DbSet<FileViewers> FileViewers { get; set; }

        public virtual DbSet<DirectoryViewers> DirectoryViewers { get; set; }

        public virtual DbSet<User> Users { get; set; }

        #endregion

        #region Methods

        public static DatabaseContext Create()
        {
            return new DatabaseContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileEntry>()
                .Map(m =>
                {
                    m.MapInheritedProperties();
                    m.ToTable("FileEntries");
                });

            modelBuilder.Entity<DirectoryEntry>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("DirectoryEntries");
            });
        }

        #endregion
    }
}