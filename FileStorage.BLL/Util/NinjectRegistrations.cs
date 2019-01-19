using FileStorage.BLL.Interfaces;
using FileStorage.BLL.Services;
using FileStorage.DAL.Interfaces;
using FileStorage.DAL.Repositories;
using Ninject.Modules;

namespace FileStorage.BLL.Util
{
    public class NinjectRegistrations : NinjectModule
    {
        private readonly IUnitOfWork _uow;
        public NinjectRegistrations(string connection)
        {
            _uow = new UnitOfWork(connection);
        }
        public override void Load()
        {
            Bind<IFileService>().To<FileService>().WithConstructorArgument("uow", _uow);
            Bind<IUserService>().To<UserService>().WithConstructorArgument("uow", _uow);
            Bind<IUserProfileService>().To<UserProfileService>().WithConstructorArgument("uow", _uow);
        }
    }
}
