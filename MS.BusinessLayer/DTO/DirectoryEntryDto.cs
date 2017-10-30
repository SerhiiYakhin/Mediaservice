using System.ComponentModel.DataAnnotations;

namespace MS.BusinessLayer.DTO
{
    public sealed class DirectoryEntryDto : ObjectEntryDto
    {
        [Required]
        [Range(0, 10)]
        public short NodeLevel { get; set; }
    }
}
