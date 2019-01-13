using FileStorage.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace FileStorage.DAL.Identity
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store) : base(store) { }
    }
}
