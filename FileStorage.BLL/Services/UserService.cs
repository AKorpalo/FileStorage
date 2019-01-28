using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.DAL.Entities;
using FileStorage.DAL.Interfaces;
using Microsoft.AspNet.Identity;

namespace FileStorage.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _database;
        public UserService(IUnitOfWork uow)
        {
            _database = uow;
        }
        public async Task<ClaimsIdentity> AuthenticateAsync(RegisterDto registerDto)
        {
            ClaimsIdentity claims = null;
            ApplicationUser user = await _database.UserManager.FindAsync(registerDto.UserName, registerDto.Password);
            if (user != null)
            {
                claims = await _database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                claims.AddClaim(new Claim("UserName", user.UserName)); 
            }

            return claims;
        }
        public async Task<OperationDetails> CreateAsync(RegisterDto registerDto)
        {
            ApplicationUser user = await _database.UserManager.FindByEmailAsync(registerDto.Email);
            if (user == null)
            {
                ApplicationUser userName = await _database.UserManager.FindByNameAsync(registerDto.UserName);
                if (userName != null)
                {
                    return new OperationDetails(false, "Користувач з таким логіном вже існує!", "UserName");
                }

                user = new ApplicationUser { Email = registerDto.Email, UserName = registerDto.UserName };

                var result = await _database.UserManager.CreateAsync(user, registerDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await _database.UserManager.AddToRoleAsync(user.Id, "user");

                UserProfile clientProfile = new UserProfile
                {
                    Id = user.Id,
                    FirstName = registerDto.FirstName,
                    SecondName = registerDto.SecondName,
                    BirthDate = registerDto.BirthDate,
                    RegisterDate = DateTime.Now,
                    MaxSize = 20000000,
                    CurrentSize = 0
                };
                _database.UserProfileRepository.Create(clientProfile);
                await _database.SaveAsync();

                return new OperationDetails(true, "Реєстрація пройшла успішно!", "");
            }
            return new OperationDetails(false, "Користувач з такою адресою вже існує!", "Email");
        }
        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
