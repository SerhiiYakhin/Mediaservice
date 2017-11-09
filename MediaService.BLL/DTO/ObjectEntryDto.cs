using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public abstract class ObjectEntryDto
    {
        public ObjectEntryDto() => Owners = new HashSet<AspNetUserDto>();

        public Guid     Id            { get; set; }

        public Guid?    ParentId      { get; set; }

        public string   Name          { get; set; }

        //public string   Discriminator { get; set; }

        public string   Thumbnail     { get; set; }

        public long     Size          { get; set; }

        public DateTime Created       { get; set; }

        public DateTime Downloaded    { get; set; }

        public DateTime Modified      { get; set; }

        public virtual ICollection<AspNetUserDto> Owners { get; set; }
    }
}
