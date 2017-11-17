using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaService.DAL.Entities
{
    [Table("DirectoryEntries")]
    public class DirectoryEntry : ObjectEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DirectoryEntry()
        {
            Viewers = new HashSet<DirectoryViewers>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DirectoryViewers> Viewers { get; set; }

        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }

        [Column("Owner_Id")]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
    }
}