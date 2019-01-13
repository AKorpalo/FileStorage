using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStorage.BLL.Interfaces
{
    public interface IServiceCreator //замінити на ninject
    {
        IUserService CreateUserService(string connection);
    }
}
