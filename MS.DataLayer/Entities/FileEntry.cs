using System.Collections.Generic;

namespace MS.DataLayer.Entities
{
    public sealed class FileEntry : ObjectEntry
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FileEntry()
        {
            Tags = new HashSet<Tag>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<Tag> Tags { get; set; }
    }
}