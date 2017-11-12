using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediaService.DAL.Entities
{
    public class ObjectViewers
    {
        [Required]
        [StringLength(250)]
        public string Link { get; set; }

        [Key, Column(Order = 0)]
        public Guid ObjectEntryId { get; set; }

        [Key, Column(Order = 1)]
        public string UserId { get; set; }

        public virtual ObjectEntry ObjectEntry { get; set; }

        public virtual User User { get; set; }
    }
}