using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
    public class UserProfileService : IUserProfileService
    {
        private readonly IUnitOfWork _database;
        public UserProfileService(IUnitOfWork uow)
        {
            _database = uow;
        }
        public async Task<OperationDetails> UpdateAsync(UserDTO userProfileDto)
        {
            UserProfile userProfile = await _database.UserProfileRepository.GetbyIdAsync(userProfileDto.Id);
            if (userProfile != null)
            {
                userProfile.FirstName = userProfileDto.FirstName;
                userProfile.SecondName = userProfileDto.SecondName;
                userProfile.BirthDate = userProfileDto.BirthDate;
                if (Math.Abs(userProfileDto.MaxSize) > 0)
                {
                    userProfile.MaxSize = userProfileDto.MaxSize;
                } 
                _database.UserProfileRepository.Update(userProfile);
                await _database.SaveAsync();
                return new OperationDetails(true, "Дані оновлені!", "");
            }
            return new OperationDetails(false, "Користувача не існує!","");
        }
        public async Task<UserDTO> GetAllDetailsByIdAsync(string id)
        {
            UserProfile userProfile = await Task.Run(()=> _database.UserProfileRepository.GetbyId(id));
            if (userProfile != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDTO>()
                    .ForMember(x => x.Email, opt => opt.MapFrom(c => c.ApplicationUser.Email))
                    .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.ApplicationUser.UserName))
                    .ForMember(x => x.Email, opt => opt.MapFrom(c => c.ApplicationUser.Email))
                    ).CreateMapper();
                return mapper.Map<UserProfile, UserDTO>(userProfile);
            }

            return null;
        }
        public async Task<UserProfileDTO> GetEditDetailsByIdAsync(string id)
        {
            UserProfile userProfile = await Task.Run(() => _database.UserProfileRepository.GetbyId(id));
            if (userProfile != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserProfileDTO>()).CreateMapper();
                return mapper.Map<UserProfile, UserProfileDTO>(userProfile);
            }

            return null;
        }
        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDTO>()
                .ForMember(x => x.Email, opt => opt.MapFrom(c => c.ApplicationUser.Email))
                .ForMember(x => x.UserName, opt => opt.MapFrom(c => c.ApplicationUser.UserName))
            ).CreateMapper();
            var list = await _database.UserProfileRepository.GetAllAsync();
            return mapper.Map<IEnumerable<UserProfile>, List<UserDTO>>(list);
        }

        public void Dispose()
        {
            _database.Dispose();
        }

        public async Task<OperationDetails> DeleteAsync(string id, string path)
        {
            ApplicationUser user = await _database.UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return new OperationDetails(false,"Такого користувача не існує","");
            }

            var list = user.Files.ToList();

            FileService fileService = new FileService(_database);

            foreach (var file in list)
            {
                await fileService.DeleteAsync(file.Id, path);
            }

            _database.UserManager.Delete(user);
            _database.UserProfileRepository.Delete(user.Id);
            await _database.SaveAsync();
            return new OperationDetails(true, "Користувач успішно видалений", "");
        }
    }
}
