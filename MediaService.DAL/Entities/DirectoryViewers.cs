#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    public class DirectoryViewers
    {
        [Required]
        [StringLength(250)]
        public string Link { get; set; }

        [Key]
        [Column("User_Id", Order = 1)]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        [Key]
        [Column("DirectoryEntry_Id", Order = 0)]
        public Guid DirectoryEntryId { get; set; }

        public virtual DirectoryEntry DirectoryEntry { get; set; }
    }
}