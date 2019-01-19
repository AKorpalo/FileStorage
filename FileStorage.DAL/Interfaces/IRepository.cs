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
        T GetbyId(string id);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetbyIdAsync(string id);
        Task CreateAsync(T item);
        Task UpdateAsync(T item);
        Task DeleteAsync(string id);
    }
}
