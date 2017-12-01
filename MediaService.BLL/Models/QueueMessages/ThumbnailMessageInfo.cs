using MediaService.BLL.Models.Enums;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class ThumbnailMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }

        [DataMember]
        public List<string> FilesNames { get; set; }
    }
}
