using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IRoleService
    {
        RolesDTO GetAllRoles();
        RolesDTO GetAllUserRoles(string id);

        OperationDetails AddRole(string userId, string roleName);
        OperationDetails DeleteRole(string userId, string roleName);
    }
}
