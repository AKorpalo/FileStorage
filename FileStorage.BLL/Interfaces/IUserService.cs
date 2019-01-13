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
        Task<OperationDetails> Create(RegisterDto registerDto);
        Task<ClaimsIdentity> Authenticate(RegisterDto registerDto);
        Task SetInitialData(RegisterDto adminDto, List<string> roles);
    }
}
