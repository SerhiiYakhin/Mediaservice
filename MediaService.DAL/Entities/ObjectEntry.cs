#region usings

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MediaService.DAL.Entities
{
    public abstract class ObjectEntry
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        //[ConcurrencyCheck]
        public string Name { get; set; }

        //[Required]
        //[StringLength(128)]
        //public string Discriminator { get; set; }

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


        public DirectoryEntry Parent { get; set; }

        [Column("Parent_Id")]
        public Guid? ParentId { get; set; }
    }
}