#region usings

using System;
using System.Collections.Generic;
using MediaService.BLL.Models.Enums;

#endregion

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