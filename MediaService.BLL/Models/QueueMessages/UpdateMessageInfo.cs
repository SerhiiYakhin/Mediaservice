#region usings

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MediaService.BLL.Models.Enums;

#endregion

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class UpdateMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }

        [DataMember]
        public Guid EntryId { get; set; }

        [DataMember]
        public List<Guid> EntriesIds { get; set; }

        [DataMember]
        public string NewName { get; set; }
    }
}