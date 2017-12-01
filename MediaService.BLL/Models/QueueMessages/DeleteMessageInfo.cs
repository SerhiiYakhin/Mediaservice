using MediaService.BLL.Models.Enums;
using System;
using System.Runtime.Serialization;

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class DeleteMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }

        [DataMember]
        public Guid EntryId { get; set; }
    }
}
