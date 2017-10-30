using Microsoft.AspNet.Identity.EntityFramework;

namespace MediaService.PL.Models.IdentityModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();
    }
}