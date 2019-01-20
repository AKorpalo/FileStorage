using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.BLL.Interfaces
{
    public interface IUnitOfWorkService : IDisposable
    {
        IFileService FileService { get;}
        IUserService UserService { get;}
        IUserProfileService UserProfileService { get;}
        IRoleService RoleService { get; }
        Task SaveAsync();
    }
}
