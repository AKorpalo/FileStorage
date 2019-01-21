using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Microsoft.AspNet.Identity;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class ManageUserProfileController : Controller
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }

        [Authorize]
        public async Task<ActionResult> GetDetails()
        {
            var user = await UnitOfWorkService.UserProfileService.GetAllDetailsByIdAsync(User.Identity.GetUserId());
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()
            ).CreateMapper();
            var model = mapper.Map<UserDTO, UserViewModel>(user);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UnitOfWorkService.UserProfileService.GetEditDetailsByIdAsync(id);
            if (user != null)
            {
                var model = new UserProfileViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    BirthDate = user.BirthDate
                };
                return View(model);
            }

            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userProfile = new UserDTO
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    BirthDate = model.BirthDate
                };
                var operationDetails = await UnitOfWorkService.UserProfileService.UpdateAsync(userProfile);
                if (operationDetails.Succedeed)
                {
                    TempData["mes"] = operationDetails.Message;
                    return RedirectToAction("GetDetails");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    return View(model);
                }
            }

            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            var result = await UnitOfWorkService.UserProfileService.DeleteAsync(id, Server.MapPath("~"));
            if (result.Succedeed)
            {
                if (User.IsInRole("admin"))
                {
                    return RedirectToAction("ShowUsers", "Admin");
                }

                return RedirectToAction("Login","Account");
            }

            return RedirectToAction("GetDetails");
        }
    }
}