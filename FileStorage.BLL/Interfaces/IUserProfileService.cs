using System;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IUserProfileService : IDisposable
    { 
        Task<OperationDetails> Update(UserProfileDTO userProfileDto);
        Task<UserDTO> GetByAllDetailsById(string id);
        Task<UserProfileDTO> GetByEditDetailsById(string id);
        Task<OperationDetails> DeleteAsync(string id, string path);
        OperationDetails Delete(string id, string path);
    }
}
