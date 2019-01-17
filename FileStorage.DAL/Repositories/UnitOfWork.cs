using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.DAL.EF;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Identity;
using FileStorage.DAL.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FileStorage.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _db;

        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationRoleManager _roleManager;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IRepository<FileData> _fileDataRepository;

        public UnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            _userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            _roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            _userProfileRepository = new UserProfileRepository(_db);
            _fileDataRepository = new FileDataRepository(_db);
        }

        public ApplicationUserManager UserManager => _userManager;
        public IRepository<UserProfile> UserProfileRepository => _userProfileRepository;
        public IRepository<FileData> FileDataRepository => _fileDataRepository;
        public ApplicationRoleManager RoleManager => _roleManager;

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _userManager.Dispose();
                    _roleManager.Dispose();
                    _userProfileRepository.Dispose();
                    _fileDataRepository.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
