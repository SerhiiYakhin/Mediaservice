#region usings

using Microsoft.AspNet.Identity.EntityFramework;

#endregion

namespace MediaService.PL.Models.IdentityModels
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", false) { }
        //public ApplicationDbContext() : base("AzureDbConnection", false) { }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}