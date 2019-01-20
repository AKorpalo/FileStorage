using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace FileStorage.BLL.Services
{
    public class RoleService : IRoleService
    {
        private IUnitOfWork _database;

        public RoleService(IUnitOfWork uow)
        {
            _database = uow;
        }

        public OperationDetails AddRole(string userId, string roleName)
        {
            var result = _database.UserManager.AddToRole(userId, roleName);
            if (result.Succeeded)
            {
                return new OperationDetails(true,"Роль успішно додана","");
            }
            return new OperationDetails(false,"Помилка","");
        }

        public OperationDetails DeleteRole(string userId, string roleName)
        {
            var result = _database.UserManager.RemoveFromRole(userId, roleName);
            if (result.Succeeded)
            {
                return new OperationDetails(true, "Роль успішно видалено", "");
            }
            return new OperationDetails(false, "Помилка", "");
        }

        public RolesDTO GetAllRoles()
        {
            var allRoles = _database.RoleManager.Roles.Select(p => p.Name).ToList();
            RolesDTO roles = new RolesDTO()
            {
                Roles = allRoles
            };
            return roles;
        }

        public RolesDTO GetAllUserRoles(string id)
        {
            var allRoles = _database.UserManager.GetRoles(id).ToList();
            RolesDTO roles = new RolesDTO()
            {
                Id = id,
                Roles = allRoles
            };
            return roles;
        }
    }
}
