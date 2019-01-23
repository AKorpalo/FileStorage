using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;

namespace FileStorage.BLL.Interfaces
{
    public interface IFileService : IDisposable
    {
        Task<IEnumerable<FileInfoDTO>> GetAllAsync();
        Task<FileInfoDTO> GetByIdAsync(string id);
        Task<OperationDetails> CreateAsync(FileInfoDTO file);
        Task<OperationDetails> UpdateAsync(FileInfoDTO item);
        Task<OperationDetails> DeleteAsync(string id,string path);
        Task<FileDownloadDTO> DownloadAsync(string id, string path);
    }
}
