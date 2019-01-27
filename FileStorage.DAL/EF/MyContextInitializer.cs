using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FileStorage.DAL.EF
{
    public class MyContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            var roles = new List<string> { "user", "admin", "moderator" };

            foreach (string roleName in roles)
            {
                var role = db.Roles.FirstOrDefault(p => p.Name == roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    roleManager.Create((ApplicationRole)role);
                }
            }

            var admin = new ApplicationUser()
            {
                Email = "KorpaloAndrew@gmail.com",
                UserName = "Weynard"
            };
            var adminProfile = new UserProfile()
            {
                Id = admin.Id,
                FirstName = "Андрій",
                SecondName = "Корпало",
                BirthDate = DateTime.Parse("26.06.1996"),
                CurrentSize = 0,
                MaxSize = 100000000,
                RegisterDate = DateTime.Now
            };
            userManager.Create(admin, "qawsed");
            userManager.AddToRole(admin.Id, "admin");
            userManager.AddToRole(admin.Id, "user");
            userManager.AddToRole(admin.Id, "moderator");
            db.UserProfiles.Add(adminProfile);



            db.SaveChanges();
        }
    }
}
