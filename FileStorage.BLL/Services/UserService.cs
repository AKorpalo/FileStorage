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
        public IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<ClaimsIdentity> Authenticate(RegisterDto registerDto)
        {
            ClaimsIdentity claims = null;
            ApplicationUser user = await Database.UserManager.FindAsync(registerDto.Email, registerDto.Password);
            if (user != null)
            {
                claims = await Database.UserManager.CreateIdentityAsync(user,
                    DefaultAuthenticationTypes.ApplicationCookie);
                claims.AddClaim(new Claim("UserName", user.UserName)); //нові Claim сюди
            }

            return claims;
        }

        public async Task<OperationDetails> Create(RegisterDto registerDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(registerDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = registerDto.Email, UserName = registerDto.UserName };
                var result = await Database.UserManager.CreateAsync(user, registerDto.Password);
                if (result.Errors.Any())
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                await Database.UserManager.AddToRoleAsync(user.Id, registerDto.Role);
                UserProfile clientProfile = new UserProfile
                {
                    Id = user.Id,
                    FirstName = registerDto.FirstName,
                    SecondName = registerDto.SecondName,
                    BirthDate = registerDto.BirthDate,
                    MaxSize = 20,
                    CurrentSize = 0,
                };
                Database.UserProfileRepository.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Реєстрація пройшла успішно!", "");
            }
            return new OperationDetails(false, "Користувач з таким логіном вже існує!", "Email");
        }

        public async Task SetInitialData(RegisterDto adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }

            await Create(adminDto);
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
