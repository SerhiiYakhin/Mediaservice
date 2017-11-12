using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public abstract class ObjectEntryDto
    {
        protected ObjectEntryDto() => Viewers = new HashSet<ObjectViewersDto>();

        public Guid Id { get; set; }

        public string Name { get; set; }

        //[Required]
        //[StringLength(128)]
        //public string Discriminator { get; set; }

        public DateTime Created { get; set; }

        public DateTime Downloaded { get; set; }

        public DateTime Modified { get; set; }

        public string Thumbnail { get; set; }

        public DirectoryEntryDto Parent { get; set; }

        public Guid? ParentId { get; set; }

        public virtual UserDto Owner { get; set; }

        public virtual ICollection<ObjectViewersDto> Viewers { get; set; }
    }
}
