using System.ComponentModel.DataAnnotations;

namespace MediaService.Models.AppModels
{
    public sealed class DirectoryEntry : ObjectEntry
    {
        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }
    }
}