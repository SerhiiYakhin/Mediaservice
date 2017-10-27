using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MS.DataLayer.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace MS.DataLayer.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            ObjectEntries = new HashSet<ObjectEntry>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public ICollection<ObjectEntry> ObjectEntries { get; set; }
    }
}
