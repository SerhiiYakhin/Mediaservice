#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    public class FileViewers
    {
        [Required]
        [StringLength(250)]
        public string Link { get; set; }

        [Key]
        [Column("FileEntry_Id", Order = 0)]
        public Guid FileEntryId { get; set; }

        [Key]
        [Column("User_Id", Order = 1)]
        public string UserId { get; set; }

        public virtual FileEntry FileEntry { get; set; }

        public virtual User User { get; set; }
    }
}