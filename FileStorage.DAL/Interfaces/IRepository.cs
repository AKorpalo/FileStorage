using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    }
}
