using System;
using System.Collections.Generic;

namespace MediaService.BLL.DTO
{
    public sealed class TagDto
    {
        public TagDto() => FileEntries = new HashSet<FileEntryDto>();

        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<FileEntryDto> FileEntries { get; set; }
    }
}
