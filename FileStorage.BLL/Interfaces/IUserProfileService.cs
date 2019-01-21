using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IUserProfileService : IDisposable
    { 
        Task<OperationDetails> UpdateAsync(UserDTO userProfileDto);
        Task<UserDTO> GetAllDetailsByIdAsync(string id);
        Task<UserProfileDTO> GetEditDetailsByIdAsync(string id);
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<OperationDetails> DeleteAsync(string id, string path);
    }
}
