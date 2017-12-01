using MediaService.BLL.Models.Enums;
using System;
using System.Collections.Generic;

namespace MediaService.BLL.Models
{
    public class MessageInfo
    {
        public OperationType OperationType { get; set; }

        public IEnumerable<Guid> EntriesIds { get; set; }

        public IEnumerable<string> EntriesNames { get; set; }

        public Guid EntryId { get; set; }
    }
}
