using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.EF;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;

namespace FileStorage.DAL.Repositories
{
    public class FileDataRepository : IRepository<FileData>
    {
        private readonly ApplicationContext _database;
        public FileDataRepository(ApplicationContext db)
        {
            _database = db;
        }

        public IEnumerable<FileData> GetAll()
        {
            return _database.FileDatas.Include(x => x.User);
        }
        public async Task<IEnumerable<FileData>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }
        public FileData GetbyId(string id)
        {
            return _database.FileDatas.FirstOrDefault(x => x.Id == id);
        }
        public async Task<FileData> GetbyIdAsync(string id)
        {
            return await _database.FileDatas.FirstOrDefaultAsync(x => x.Id == id);
        }
        public void Create(FileData item)
        {
            _database.FileDatas.Add(item);
        }
        public async Task CreateAsync(FileData item)
        {
            await Task.Run(() => Create(item));
        }
        public void Delete(string id)
        {
            var item = _database.FileDatas.FirstOrDefault(p => p.Id == id);
            if (item != null)
                _database.FileDatas.Remove(item);
        }
        public async Task DeleteAsync(string id)
        {
            await Task.Run(() => Delete(id));
        }
        public void Update(FileData item)
        {
            _database.Entry(item).State = EntityState.Modified;
        }
        public async Task UpdateAsync(FileData item)
        {
            await Task.Run(() => Update(item));
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
