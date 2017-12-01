#region usings

using System.Collections.Generic;
using System.Runtime.Serialization;
using MediaService.BLL.Models.Enums;

#endregion

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