using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediaService.Models.AppModels
{
    public sealed partial class Tag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tag()
        {
            Objects = new HashSet<Object>();
        }

        public int TagId { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Object> Objects { get; set; }
    }
}
