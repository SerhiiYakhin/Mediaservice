#region usings

using System;
using System.Collections.Generic;

#endregion

namespace MediaService.BLL.DTO
{
    public class TagDto
    {
        public TagDto()
        {
            FileEntries = new HashSet<FileEntryDto>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<FileEntryDto> FileEntries { get; set; }
    }
}