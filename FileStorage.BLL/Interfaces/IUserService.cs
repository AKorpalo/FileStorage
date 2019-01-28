using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> CreateAsync(RegisterDto registerDto);
        Task<ClaimsIdentity> AuthenticateAsync(RegisterDto registerDto);
    }
}
