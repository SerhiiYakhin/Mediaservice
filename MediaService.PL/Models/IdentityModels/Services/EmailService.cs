#region usings

using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

#endregion

namespace MediaService.PL.Models.IdentityModels.Services
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}