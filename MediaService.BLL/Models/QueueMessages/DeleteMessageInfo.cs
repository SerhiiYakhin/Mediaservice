#region usings

using System;
using System.Runtime.Serialization;
using MediaService.BLL.Models.Enums;

#endregion

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