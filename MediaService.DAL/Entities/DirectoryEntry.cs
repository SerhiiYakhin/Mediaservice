using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaService.DAL.Entities
{
    [Table("DirectoryEntries")]
    public sealed class DirectoryEntry : ObjectEntry
    {
        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }
    }
}