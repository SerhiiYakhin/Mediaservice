using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaService.DAL.Entities
{
    public abstract class ObjectEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ObjectEntry() => Owners = new HashSet<AspNetUser>();

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid  Id { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        //[Required]
        //[StringLength(128)]
        //public string Discriminator { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Created { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Downloaded { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime Modified { get; set; }

        [StringLength(250)]
        public string Thumbnail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> Owners { get; set; }
    }
}