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
        public IUserProfileService UserProfileService { get; set; }

        [Authorize]
        public async Task<ActionResult> GetDetails()
        {
            var user = await UserProfileService.GetByAllDetailsById(User.Identity.GetUserId());
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()
            ).CreateMapper();
            var model = mapper.Map<UserDTO, UserViewModel>(user);
            return View(model);
        }

        [Authorize]
        public ActionResult Edit(string id)
        {
            var user = UserProfileService.GetByEditDetailsById(id).Result;
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
                UserProfileDTO userProfile = new UserProfileDTO
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    BirthDate = model.BirthDate
                };
                var operationDetails = await UserProfileService.Update(userProfile);
                if (operationDetails.Succedeed)
                {
                    RedirectToAction("Edit");
                }
                else
                {
                    ModelState.AddModelError(operationDetails.Property, operationDetails.Message);
                    return View(model);
                }
            }

            return RedirectToAction("GetDetails");
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            var result = UserProfileService.Delete(id, Server.MapPath("~"));
            if (result.Succedeed)
            {
                return RedirectToAction("Login","Account");
            }

            return RedirectToAction("GetDetails");
        }
    }
}