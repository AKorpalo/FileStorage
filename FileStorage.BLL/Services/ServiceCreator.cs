using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Repositories;

namespace FileStorage.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new UnitOfWork(connection));
        }
    }
}
