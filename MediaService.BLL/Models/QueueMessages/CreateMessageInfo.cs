using MediaService.BLL.Models.Enums;
using System.Runtime.Serialization;

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class CreateMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }
    }
}
