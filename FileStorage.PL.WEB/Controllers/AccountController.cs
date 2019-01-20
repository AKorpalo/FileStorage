using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Microsoft.Owin.Security;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class AccountController : Controller
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            await SetInitialDataAsync();
            if (ModelState.IsValid)
            {
                RegisterDto registerDto = new RegisterDto { UserName = model.Login, Password = model.Password };
                ClaimsIdentity claims = await UnitOfWorkService.UserService.Authenticate(registerDto);
                if (claims == null)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль!");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claims);
                    //ViewData["Mes"] = "Вітаю" + model.Login;
                    return RedirectToAction("GetAll", "File");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            await SetInitialDataAsync();

            if (ModelState.IsValid)
            {
                RegisterDto registerDto = new RegisterDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    BirthDate = model.BirthDate,
                    UserName = model.UserName,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName
                };
                OperationDetails operationDetails = await UnitOfWorkService.UserService.Create(registerDto);
                if (operationDetails.Succedeed)
                { 
                    //ViewBag.Message = "Користувача " + model.UserName + " успішно зареєстровано!";
                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
            }

            return View(model);
        }

        public ActionResult Logoff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private async Task SetInitialDataAsync()
        {
            await UnitOfWorkService.UserService.SetInitialData(new RegisterDto()
            {
                Email = "KorpaloAndrew@gmail.com",
                UserName = "Weynard",
                Password = "qawsed",
                FirstName = "Андрій",
                SecondName = "Корпало",
                BirthDate = DateTime.Parse("26.06.1996"),
                Role = "admin",
            }, new List<string> { "user", "admin", "moderator"});
        }
    }
}