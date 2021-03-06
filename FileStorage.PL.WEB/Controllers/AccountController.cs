﻿using System;
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
    public class AccountController : LangController
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Login()
        {
            Logoff();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                RegisterDto registerDto = new RegisterDto { UserName = model.Login, Password = model.Password };
                ClaimsIdentity claims = await UnitOfWorkService.UserService.AuthenticateAsync(registerDto);
                if (claims == null)
                {
                    ModelState.AddModelError("", "Невірний логін або пароль!");
                }
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        //IsPersistent = true
                    }, claims);
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
                OperationDetails operationDetails = await UnitOfWorkService.UserService.CreateAsync(registerDto);
                if (operationDetails.Succedeed)
                {
                    TempData["SuccessMessage"] = "Користувача " + model.UserName + " успішно зареєстровано!";
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
    }
}