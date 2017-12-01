#region usings

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    [Table("AspNetUsers")]
    public class User
    {
        public User()
        {
            Files = new HashSet<FileEntry>();
            Directories = new HashSet<DirectoryEntry>();
            SharedFiles = new HashSet<FileViewers>();
            SharedDirectories = new HashSet<DirectoryViewers>();
        }

        public string Id { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }


        public virtual UserProfile UserProfile { get; set; }

        public virtual ICollection<FileViewers> SharedFiles { get; set; }

        public virtual ICollection<DirectoryViewers> SharedDirectories { get; set; }

        public virtual ICollection<FileEntry> Files { get; set; }

        public virtual ICollection<DirectoryEntry> Directories { get; set; }
    }
}