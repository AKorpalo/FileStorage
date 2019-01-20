using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Interfaces;

namespace FileStorage.BLL.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private IUnitOfWork _database;
        private IFileService _fileService;
        private IUserService _userService;
        private IUserProfileService _userProfileService;
        private IRoleService _roleService;
        public UnitOfWorkService(IUnitOfWork uow)
        {
            _database = uow;
            _fileService = new FileService(_database);
            _userService = new UserService(_database);
            _userProfileService = new UserProfileService(_database);
            _roleService = new RoleService(_database);
        }

        public IFileService FileService => _fileService;
        public IUserService UserService => _userService;
        public IUserProfileService UserProfileService => _userProfileService;
        public IRoleService RoleService => _roleService;
        public async Task SaveAsync()
        {
            await _database.SaveAsync();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
