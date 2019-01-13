using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileStorage.DAL.Entities;

namespace FileStorage.DAL.Interfaces
{
    public interface IUserProfileRepository : IDisposable
    {
        IEnumerable<UserProfile> GetAll();
        UserProfile GetbyId(string id);
        void Create(UserProfile item);
        void Update(UserProfile item);
        void Delete(string id);

    }
}
