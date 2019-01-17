using System.Data.Entity;
using FileStorage.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FileStorage.DAL.EF
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string connectionString) : base(connectionString)
        {
            var ensureDllIsCopied = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<FileData> FileDatas { get; set; }
    }
}
