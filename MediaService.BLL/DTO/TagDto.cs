#region usings

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    [DataContract]
    public class TagDto
    {
        public TagDto()
        {
            FileEntries = new HashSet<FileEntryDto>();
        }

        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ICollection<FileEntryDto> FileEntries { get; set; }
    }
}