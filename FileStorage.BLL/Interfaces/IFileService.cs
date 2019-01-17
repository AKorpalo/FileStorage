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
        Task<OperationDetails> Create(FileDTO file);
        Task<IEnumerable<FileInfoDTO>> GetAllAsync();
        IEnumerable<FileInfoDTO> GetAll();
        Task<FileInfoDTO> GetByIdAsync(string id);
        FileInfoDTO GetById(string id);
    }
}
