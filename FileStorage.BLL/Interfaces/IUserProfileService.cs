using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IUserProfileService : IDisposable
    { 
        Task<OperationDetails> Update(UserDTO userProfileDto);
        Task<UserDTO> GetAllDetailsById(string id);
        Task<UserProfileDTO> GetEditDetailsById(string id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<OperationDetails> DeleteAsync(string id, string path);
        OperationDetails Delete(string id, string path);
    }
}
