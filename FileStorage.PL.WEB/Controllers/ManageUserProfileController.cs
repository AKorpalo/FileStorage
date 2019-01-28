using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Microsoft.AspNet.Identity;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class ManageUserProfileController : LangController
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }

        [Authorize]
        public async Task<ActionResult> GetDetails()
        {
            var user = await UnitOfWorkService.UserProfileService.GetAllDetailsByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                TempData["ErrorMessage"] = "Помилка, спробуйте пізніше!";
                return RedirectToAction("GetAll", "File");
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()
            ).CreateMapper();
            var model = mapper.Map<UserDTO, UserViewModel>(user);
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await UnitOfWorkService.UserProfileService.GetAllDetailsByIdAsync(id);
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

            TempData["ErrorMessage"] = "Помилка, спробуйте пізніше!";
            return RedirectToAction("GetDetails");
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
                OperationDetails result;
                try
                {
                    result = await UnitOfWorkService.UserProfileService.UpdateAsync(userProfile);
                }
                catch
                {
                    return View("Error");
                }

                if (result.Succedeed)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("GetDetails");
                }
                TempData["ErrorMessage"] = result.Message;
                return View(model);
            }

            return View(model);
        }

        [Authorize]
        public ActionResult _Delete(string id)
        {
            return PartialView("_Delete",(object)id);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Delete(string id)
        {
            OperationDetails result;
            try
            {
                result = await UnitOfWorkService.UserProfileService.DeleteAsync(id, Server.MapPath("~"));
            }
            catch
            {
                TempData["ErrorMessage"] = "Помилка, спробуйте пізніше!";
                return RedirectToAction("GetDetails");
            }
            if (result.Succedeed)
            {
                if (User.IsInRole("admin"))
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("ShowUsers", "Admin");
                } 
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("Logoff","Account");
            }
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("GetDetails");
        }
    }
}