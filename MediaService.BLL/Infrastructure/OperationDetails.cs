using Microsoft.AspNet.Identity;

namespace MediaService.BLL.Infrastructure
{
    public class OperationDetails : IdentityResult
    {
        public OperationDetails(bool succedeed, string message, string prop) : base(succedeed)
        {
            Succedeed = succedeed;
            Message = message;
            Property = prop;
        }

        public bool   Succedeed { get; }

        public string Message   { get; }

        public string Property  { get; }
    }
}
