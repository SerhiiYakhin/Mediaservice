#region usings

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MediaService.DAL.Entities.Enums;

#endregion

namespace MediaService.DAL.Entities
{
    [Table("FileEntries")]
    public class FileEntry : ObjectEntry
    {
        public FileEntry()
        {
            Viewers = new HashSet<FileViewers>();
            Tags = new HashSet<Tag>();
        }

        [Required]
        [StringLength(128)]
        //[ConcurrencyCheck]
        public string Name { get; set; }

        [Required]
        public int Size { get; set; }

        [StringLength(256)]
        public string FileThumbnailLink { get; set; }

        [Required]
        public FileType FileType { get; set; }

        [Column("Owner_Id")]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public virtual ICollection<FileViewers> Viewers { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}