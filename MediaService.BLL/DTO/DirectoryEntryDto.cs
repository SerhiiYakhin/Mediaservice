using System.Collections.Generic;
using MediaService.DAL.Entities;

namespace MediaService.BLL.DTO
{
    public class DirectoryEntryDto : ObjectEntryDto
    {
        public DirectoryEntryDto()
        {
            Viewers = new HashSet<DirectoryViewersDto>();
        }

        public virtual ICollection<DirectoryViewersDto> Viewers { get; set; }

        public short NodeLevel { get; set; }

        public virtual User Owner { get; set; }
    }
}
