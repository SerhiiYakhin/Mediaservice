#region usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    [Table("DirectoryEntries")]
    public class DirectoryEntry : ObjectEntry
    {
        public DirectoryEntry()
        {
            Viewers = new HashSet<DirectoryViewers>();
        }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }

        [Column("Owner_Id")]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<DirectoryViewers> Viewers { get; set; }
    }
}