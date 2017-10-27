using System.ComponentModel.DataAnnotations;

namespace MS.DataLayer.Entities
{
    public sealed class DirectoryEntry : ObjectEntry
    {
        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }
    }
}