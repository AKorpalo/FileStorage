using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.Identity;

namespace FileStorage.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IUserProfileRepository UserProfileRepository { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
