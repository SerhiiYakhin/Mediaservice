using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MS.BusinessLayer.DTO
{
    public sealed class TagDto
    {
        public TagDto() => FileEntries = new HashSet<FileEntryDto>();

        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("TagId")]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public ICollection<FileEntryDto> FileEntries { get; set; }
    }
}
