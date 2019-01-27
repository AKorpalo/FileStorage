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
    public class AdminController : LangController
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        private int _numberOfObjectsPerPage = 2;

        public async Task<ActionResult> ShowUsers()
        {
            var list = await UnitOfWorkService.UserProfileService.GetAllUsersAsync();
            var fileInfoList = list.ToList();

            var pages = fileInfoList.Count;

            if (pages % _numberOfObjectsPerPage != 0)
            {
                pages /= _numberOfObjectsPerPage;
                pages++;
            }
            else
            {
                pages /= _numberOfObjectsPerPage;
            }

            var fileInfoDtos = fileInfoList.Take(_numberOfObjectsPerPage).ToList();
            var model = new TableViewModel<UserDTO>()
            {
                Items = fileInfoDtos,
                Pages = pages
            };
            return View(model);
        }

        public async Task<PartialViewResult> _Search(string searchString)
        {
            var list = await UnitOfWorkService.UserProfileService.GetAllUsersAsync();
            var fileInfoList = list .Where(f => f.UserName.ToLower().Contains(searchString.ToLower())).ToList();

            var pages = fileInfoList.Count;

            if (pages % _numberOfObjectsPerPage != 0)
            {
                pages /= _numberOfObjectsPerPage;
                pages++;
            }
            else
            {
                pages /= _numberOfObjectsPerPage;
            }

            var fileInfoDtos = fileInfoList.Take(_numberOfObjectsPerPage).ToList();
            var serchModel = new TableViewModel<UserDTO>()
            {
                Items = fileInfoDtos,
                Pages = pages,
                SearchString = searchString
            };
            return PartialView("_Table", serchModel);
        }

        public async Task<ActionResult> _Pages(PagesViewModel viewModel)
        {
            var model = viewModel;
            if (model.SearchString == null)
            {
                model.SearchString = "";
            }

            var list = await UnitOfWorkService.UserProfileService.GetAllUsersAsync();
            var fileInfoList = list.ToList();

            var pages = fileInfoList.Count;

            if (pages % _numberOfObjectsPerPage != 0)
            {
                pages /= _numberOfObjectsPerPage;
                pages++;
            }
            else
            {
                pages /= _numberOfObjectsPerPage;
            }
            var fileInfoDtos = fileInfoList.Where(f => f.UserName.ToLower().Contains(model.SearchString.ToLower()))
                .Skip(_numberOfObjectsPerPage * model.Pages)
                .Take(_numberOfObjectsPerPage).ToList();

            var serchModel = new TableViewModel<UserDTO>()
            {
                Items = fileInfoDtos,
                Pages = pages,
                SearchString = model.SearchString
            };
            return PartialView("_TableBody", serchModel);
        }

        public async Task<ActionResult> EditUser(string userId)
        {
            var details = await UnitOfWorkService.UserProfileService.GetAllDetailsByIdAsync(userId);
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
            var result = await UnitOfWorkService.UserProfileService.UpdateAsync(user);
            if (result.Succedeed)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("ShowUsers");
            }
            TempData["ErrorMessage"] = result.Message;
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
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("AddRole",new{ userId = model.Id});
            }
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("AddRole", new { userId = model.Id });
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteRole(string userId, string role)
        {
            var result = UnitOfWorkService.RoleService.DeleteRole(userId, role);
            if (result.Succedeed)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("AddRole", "Admin", new {userId});
            }
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("AddRole","Admin",new{ userId });
        }
    }
}