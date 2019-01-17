﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;

namespace FileStorage.BLL.Services
{
    class FileService : IFileService
    {
        private readonly IUnitOfWork _database;

        public FileService(IUnitOfWork uow)
        {
            _database = uow;
        }

        public async Task<OperationDetails> Create(FileDTO file)
        {
            ApplicationUser user = await _database.UserManager.FindByIdAsync(file.UserId);
            if (user == null)
            {
                return new OperationDetails(false, "Користувача з таким Email не існує", "");
            }

            string saveFileName = Guid.NewGuid().ToString() + ".temp";

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
            _database.FileDataRepository.Create(fileData);
            await _database.SaveAsync();
            return new OperationDetails(true, "Файл успішно збережено!", "");
        }
        public async Task<IEnumerable<FileInfoDTO>> GetAllAsync()
        {
            return await Task.Run(() => GetAll());
        }
        public IEnumerable<FileInfoDTO> GetAll()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileData, FileInfoDTO>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(c => c.User.UserName))).CreateMapper();
            var list = _database.FileDataRepository.GetAll();
            return mapper.Map<IEnumerable<FileData>, List<FileInfoDTO>>(list);
        }
        public async Task<FileInfoDTO> GetByIdAsync(string id)
        {
            return await Task.Run(() => GetById(id));
        }
        public FileInfoDTO GetById(string id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<FileData, FileInfoDTO>()
                .ForMember(x => x.RelativePath, opt => opt.MapFrom(c => c.FilePath))).CreateMapper();
            var item = _database.FileDataRepository.GetbyId(id);
            if (item != null)
            {
                return mapper.Map<FileData, FileInfoDTO>(item);
            }

            return null;
        }
        public void Dispose()
        {
            _database.Dispose();
        }

        private OperationDetails SaveFile(Stream inputStream, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    inputStream.CopyTo(fileStream);
                    return new OperationDetails(true, "Файл збережено успішно", "");
                }
                catch (Exception ex)
                {
                    return new OperationDetails(false, "Не вдалося зберегти файл!", "");
                }
            }
        }
        private async Task<OperationDetails> SaveFileAsync(Stream inputStream, string filePath)
        {
            return await Task.Run(() => SaveFile(inputStream, filePath));
        }
        public FileDownloadDTO CheckDownlod(string id, string path, string userId)
        {
            var fileInfo = FileIsFound(id, path);
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
        private FileInfoDTO FileIsFound(string id, string path)
        {
            FileInfoDTO fileInfo = GetById(id);
            if (fileInfo != null)
            {
                if (File.Exists(Path.Combine(path, fileInfo.RelativePath.Substring(2, fileInfo.RelativePath.Length - 2))))
                {
                    return fileInfo;
                }
                else
                {
                    _database.FileDataRepository.Delete(fileInfo.Id);
                    _database.SaveAsync();
                    return null;
                }
            }

            return null;

        }
    }
}