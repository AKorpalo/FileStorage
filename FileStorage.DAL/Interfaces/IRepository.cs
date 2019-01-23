using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetbyId(string id);
        Task<T> GetbyIdAsync(string id);
        void Create(T item);
        Task CreateAsync(T item);
        void Update(T item);
        Task UpdateAsync(T item);
        void Delete(string id);
        Task DeleteAsync(string id);
    }
}
