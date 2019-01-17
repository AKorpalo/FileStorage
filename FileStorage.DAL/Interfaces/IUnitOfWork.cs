using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Identity;

namespace FileStorage.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IRepository<UserProfile> UserProfileRepository { get; }
        IRepository<FileData> FileDataRepository { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
