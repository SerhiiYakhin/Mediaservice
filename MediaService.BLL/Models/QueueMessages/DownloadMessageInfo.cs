#region usings

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MediaService.BLL.Models.Enums;

#endregion

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class DownloadMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }

        [DataMember]
        public List<Guid> EntriesIds { get; set; }

        [DataMember]
        public Guid ZipId { get; set; }
    }
}