using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FileStorage.DAL.EF;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;

namespace FileStorage.DAL.Repositories
{
    class UserProfileRepository : IUserProfileRepository
    {
        public ApplicationContext Database { get; set; }

        public UserProfileRepository(ApplicationContext db)
        {
            Database = db;
        }
        public void Create(UserProfile item)
        {
            Database.UserProfiles.Add(item);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return Database.UserProfiles.Include(p => p.ApplicationUser);
        }

        public UserProfile GetbyId(string id)
        {
            return Database.UserProfiles.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile item)
        {
            Database.Entry(item).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            var item = Database.UserProfiles.FirstOrDefault(p => p.Id == id);
            if (item != null)
                Database.UserProfiles.Remove(item);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
