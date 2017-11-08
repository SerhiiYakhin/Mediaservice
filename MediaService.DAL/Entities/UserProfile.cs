using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaService.DAL.Entities
{
    public class UserProfile
    {
        public UserProfile() => ObjectEntries = new HashSet<ObjectEntry>();

        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id       { get; set; }

        [StringLength(60)]
        public string Avatar   { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ObjectEntry> ObjectEntries { get; set; }
    }
}
