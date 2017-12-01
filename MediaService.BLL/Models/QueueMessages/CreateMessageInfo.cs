#region usings

using System.Runtime.Serialization;
using MediaService.BLL.Models.Enums;

#endregion

namespace MediaService.BLL.Models.QueueMessages
{
    [DataContract]
    public class CreateMessageInfo
    {
        [DataMember]
        public OperationType OperationType { get; set; }
    }
}