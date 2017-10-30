using System.ComponentModel.DataAnnotations;

namespace MediaService.DAL.Entities
{
    public sealed class DirectoryEntry : ObjectEntry
    {
        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }
    }
}