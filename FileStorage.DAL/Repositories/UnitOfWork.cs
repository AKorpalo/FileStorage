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

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IUserProfileRepository userProfileRepository;

        public UnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(_db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(_db));
            userProfileRepository = new UserProfileRepository(_db);
        }

        public ApplicationUserManager UserManager => userManager;

        public IUserProfileRepository UserProfileRepository => userProfileRepository;

        public ApplicationRoleManager RoleManager => roleManager;

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
                    userManager.Dispose();
                    roleManager.Dispose();
                    userProfileRepository.Dispose();
                }
                _disposed = true;
            }
        }
    }
}
