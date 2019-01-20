using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Interfaces;
using FileStorage.BLL.Services;
using FileStorage.PL.WEB.Models;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class AdminController : Controller
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        public async Task<ActionResult> ShowUsers()
        {
            var model = await UnitOfWorkService.UserProfileService.GetAllUsersAsync();
            return View(model);
        }
        public async Task<PartialViewResult> _Update(string search)
        {
            var model = await UnitOfWorkService.UserProfileService.GetAllUsersAsync();
            var serchModel = model.Where(f => f.UserName.Contains(search));
            return PartialView("_TableBody", serchModel);
        }
        public async Task<ActionResult> EditUser(string userId)
        {
            var details = await UnitOfWorkService.UserProfileService.GetEditDetailsById(userId);
            UserProfileViewModel model = new UserProfileViewModel
            {
                Id = details.Id,
                FirstName = details.FirstName,
                SecondName = details.SecondName,
                BirthDate = details.BirthDate,
                MaxSize = details.MaxSize
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserProfileViewModel model)
        {
            UserDTO user = new UserDTO
            {
                Id = model.Id,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                BirthDate = model.BirthDate,
                MaxSize = model.MaxSize
            };
            var result = await UnitOfWorkService.UserProfileService.Update(user);
            if (result.Succedeed)
            {
                return RedirectToAction("ShowUsers");
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public ActionResult AddRole(string userId)
        {
            var model = UnitOfWorkService.RoleService.GetAllUserRoles(userId);
            var roles = UnitOfWorkService.RoleService.GetAllRoles();
            ViewBag.Roles = roles.Roles;
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public ActionResult AddRole(RoleViewModel model)
        {
            var result = UnitOfWorkService.RoleService.AddRole(model.Id, model.RoleName);
            if (result.Succedeed)
            {
                return AddRole(model.Id);
            }
            return AddRole(model.Id);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult _Roles(string userId, string role)
        {
            UnitOfWorkService.RoleService.DeleteRole(userId, role);
            return RedirectToAction("AddRole","Admin",new{ userId = userId });
        }
    }
}