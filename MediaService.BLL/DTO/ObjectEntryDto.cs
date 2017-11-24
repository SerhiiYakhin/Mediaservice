#region usings

using System;
using System.Runtime.Serialization;

#endregion

namespace MediaService.BLL.DTO
{
    public abstract class ObjectEntryDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public DateTime Created { get; set; }

        [DataMember]
        public DateTime Downloaded { get; set; }

        [DataMember]
        public DateTime Modified { get; set; }

        [DataMember]
        public DirectoryEntryDto Parent { get; set; }

        [DataMember]
        public Guid? ParentId { get; set; }
    }
}