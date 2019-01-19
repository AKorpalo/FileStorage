using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FileStorage.DAL.EF;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;

namespace FileStorage.DAL.Repositories
{
    class UserProfileRepository : IRepository<UserProfile>
    {
        private readonly ApplicationContext _database; 

        public UserProfileRepository(ApplicationContext db)
        {
            _database = db;
        }
        public void Create(UserProfile item)
        {
            _database.UserProfiles.Add(item);
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return _database.UserProfiles.Include(p => p.ApplicationUser);
        }

        public UserProfile GetbyId(string id)
        {
            return _database.UserProfiles.FirstOrDefault(p => p.Id == id);
        }

        public void Update(UserProfile item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }

        public void Delete(string id)
        {
            var item = _database.UserProfiles.FirstOrDefault(p => p.Id == id);
            if (item != null)
                _database.UserProfiles.Remove(item);
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public async Task<IEnumerable<UserProfile>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }

        public async Task<UserProfile> GetbyIdAsync(string id)
        {
            return await Task.Run(() => GetbyId(id));
        }

        public async Task CreateAsync(UserProfile item)
        {
            await Task.Run(() => Create(item));
        }

        public async Task UpdateAsync(UserProfile item)
        {
            await Task.Run(() => Update(item));
        }

        public async Task DeleteAsync(string id)
        {
            await Task.Run(() => Delete(id));
        }
    }
}
