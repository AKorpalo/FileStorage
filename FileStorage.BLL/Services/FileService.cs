using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace FileStorage.BLL.Services
{
    class FileService : IFileService
    {
        private readonly IUnitOfWork _database;
        public FileService(IUnitOfWork uow)
        {
            _database = uow;
        }

        public async Task<IEnumerable<FileInfoDTO>> GetAllAsync()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileData, FileInfoDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(x => x.Size, opt => opt.MapFrom(c => c.Size / 1000000))
            ).CreateMapper();
            var list = await _database.FileDataRepository.GetAllAsync();
            return mapper.Map<IEnumerable<FileData>, List<FileInfoDTO>>(list);
        }
        public async Task<FileInfoDTO> GetByIdAsync(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileData, FileInfoDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.User.UserName))
                .ForMember(x => x.Size, opt => opt.MapFrom(c => c.Size / 1000000))
                .ForMember(x => x.RelativePath, opt => opt.MapFrom(c => c.FilePath))).CreateMapper();
            var item = await _database.FileDataRepository.GetbyIdAsync(id);
            if (item != null)
            {
                return mapper.Map<FileData, FileInfoDTO>(item);
            }

            return null;
        }
        public async Task<OperationDetails> CreateAsync(FileInfoDTO file)
        {
            ApplicationUser user = await _database.UserManager.FindByIdAsync(file.UserId);

            if (user == null)
            {
                return new OperationDetails(false, "Користувача з таким Email не існує", "");
            }

            if (user.UserProfile.CurrentSize + file.Size > user.UserProfile.MaxSize)
            {
                return new OperationDetails(false, "Не вистачає місця у сховищі", "");
            }

            string saveFileName = Guid.NewGuid() + ".temp";

            var result = await SaveFileAsync(file.InputStream, file.FilePath + saveFileName);

            if (!result.Succedeed)
            {
                return result;
            }

            var fileData = new FileData()
            {
                Id = Guid.NewGuid().ToString(),
                FileName = file.FileName,
                Size = file.Size,
                DownloadDate = file.DownloadDate,
                IsPrivate = file.IsPrivate,
                FilePath = file.RelativePath + saveFileName,
                User = user,
                UserId = user.Id
            };
            user.UserProfile.CurrentSize += fileData.Size;
            try
            {
                _database.UserManager.Update(user);
            }
            catch (Exception ex)
            {
                File.Delete(file.FilePath + saveFileName);
                throw;
            }


            try
            {
                _database.FileDataRepository.Create(fileData);
            }
            catch (Exception e)
            {
                File.Delete(file.FilePath + saveFileName);
                throw;
            }
            
            await _database.SaveAsync();
            return new OperationDetails(true, "Файл успішно збережено!", "");
        }
        public async Task<OperationDetails> UpdateAsync(FileInfoDTO item)
        {
            FileData fileData = await _database.FileDataRepository.GetbyIdAsync(item.Id);
            if (fileData == null)
            {
                return new OperationDetails(false, "Файл не знайдений", "");
            }

            fileData.IsPrivate = item.IsPrivate;
            _database.FileDataRepository.Update(fileData);
            await _database.SaveAsync();
            return new OperationDetails(true, "Файл успішно змінено", "");
        }
        public async Task<OperationDetails> DeleteAsync(string id, string path)
        {
            var file = await FileIsFoundAsync(id, path);
            if (file != null)
            {
                try
                {
                    File.Delete(Path.Combine(path, file.RelativePath.Substring(2, file.RelativePath.Length - 2)));
                    await DeleteFileInfoFromDatabaseAsync(file.Id);
                }
                catch
                {
                    return new OperationDetails(false, "Файл не видалено", "");
                }
            }

            return new OperationDetails(true, "Файл успішно видалено", "");
        }
        public async Task<FileDownloadDTO> DownloadAsync(string id, string path)
        {
            FileInfoDTO fileInfo = await FileIsFoundAsync(id, path);
            if (fileInfo != null)
            {
                return new FileDownloadDTO()
                {
                    FileName = fileInfo.FileName,
                    Path = fileInfo.RelativePath,
                    IsDownload = new OperationDetails(true, "", "")
                };
            }

            return new FileDownloadDTO() { IsDownload = new OperationDetails(false, "Файл не існує!", "") };
        }

        private async Task<OperationDetails> SaveFileAsync(Stream inputStream, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    await inputStream.CopyToAsync(fileStream);
                    return new OperationDetails(true, "Файл збережено успішно", "");
                }
                catch
                {
                    return new OperationDetails(false, "Не вдалося зберегти файл!", "");
                }
            }
        }
        private async Task<FileInfoDTO> FileIsFoundAsync(string id, string path)
        {
            FileInfoDTO fileInfo;
            try
            {
                fileInfo = await GetByIdAsync(id);
            }
            catch
            {
                return null;
            }

            if (fileInfo != null)
            {
                if (File.Exists(Path.Combine(path, fileInfo.RelativePath.Substring(2, fileInfo.RelativePath.Length - 2))))
                {
                    return fileInfo;
                }
                await DeleteFileInfoFromDatabaseAsync(fileInfo.Id);
                return null;
            }

            return null;
        }
        private async Task DeleteFileInfoFromDatabaseAsync(string fileId)
        {
            FileData file = _database.FileDataRepository.GetbyId(fileId);
            if (file != null)
            {
                var user = _database.UserProfileRepository.GetbyId(file.UserId);
                if (user.CurrentSize - file.Size < 0)
                {
                    user.CurrentSize = 0;
                }
                else
                {
                    user.CurrentSize -= file.Size;
                }

                _database.UserProfileRepository.Update(user);
                _database.FileDataRepository.Delete(file.Id);
                await _database.SaveAsync();
            }
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
