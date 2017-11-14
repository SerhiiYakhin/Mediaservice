//using Microsoft.AspNet.Identity;

namespace MediaService.BLL.Infrastructure
{
    public class OperationDetails //: IdentityResult
    {
        public OperationDetails(bool succeeded, string message, string prop) //: base(succedeed)
        {
            Succeeded = succeeded;
            Message = message;
            Property = prop;
        }

        public bool Succeeded { get; }

        public string Message { get; }

        public string Property { get; }
    }
}
