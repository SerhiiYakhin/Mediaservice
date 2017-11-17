﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int Size { get; set; }

        [Column("Owner_Id")]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }
        
        public virtual ICollection<FileViewers> Viewers { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}