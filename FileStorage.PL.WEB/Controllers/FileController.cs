using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Infrastucture;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Microsoft.AspNet.Identity;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class FileController : Controller
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create(FileStateViewModel model, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    var str = "~/App_Data/Files/";
                    FileInfoDTO fileDto = new FileInfoDTO()
                    {
                        FileName = file.FileName,
                        Size = file.ContentLength,
                        DownloadDate = DateTime.Now,
                        IsPrivate = model.IsPrivate,
                        UserId = User.Identity.GetUserId(),
                        RelativePath = str,
                        FilePath = Server.MapPath(str),
                        InputStream = file.InputStream
                    };

                    var result = await UnitOfWorkService.FileService.CreateAsync(fileDto);

                    if (result.Succedeed)
                    {
                        return RedirectToAction("Getall", "File");
                    }
                }
                else
                {
                    ModelState.AddModelError("Файл не завантажений!","");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var model = list.Where(p => p.UserId == User.Identity.Name);
            return View(model);
        }

        [Authorize]
        public async Task<PartialViewResult> _GetAll(string search)
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var model = list.Where(p => p.UserId == User.Identity.Name);
            var serchModel = model.Where(f => f.FileName.Contains(search));
            return PartialView("_TableBody", serchModel);
        }
        public async Task<ActionResult> Download(string id)
        {
            var fileDownloadInfo = await UnitOfWorkService.FileService.DownloadAsync(id, Server.MapPath("~"));
            if (fileDownloadInfo.IsDownload.Succedeed)
            {
                string fullPath = Server.MapPath(fileDownloadInfo.Path);

                string fileType = MimeMapping.GetMimeMapping(fileDownloadInfo.FileName);

                string fileName = fileDownloadInfo.FileName;

                return File(fullPath, fileType, fileName);
            }
            return RedirectToAction("GetAll", "File");
        }
        public async Task<ActionResult> _Download(string id)
        {
            string c = await Task.Run(()=>BitlyApi.GetShortenedUrl(Url.Action("Download","File",new {id}, Request.Url.Scheme)));
            if (c != null)
            {
                var model = new ShortLinkViewModel
                {
                    Id = id,
                    ShortLink = c
                };
                return PartialView(model);
            }

            return HttpNotFound();
        }
        public async Task<ActionResult> _Edit(string id)
        {
            var file = await UnitOfWorkService.FileService.GetByIdAsync(id);
            FileStateViewModel model = new FileStateViewModel
            {
                Id = file.Id,
                IsPrivate = file.IsPrivate
            }; 
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(FileStateViewModel model)
        {
            FileInfoDTO file = new FileInfoDTO
            {
                Id = model.Id,
                IsPrivate = model.IsPrivate
            };
            var result = await UnitOfWorkService.FileService.UpdateAsync(file);
            if (result.Succedeed)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("GetAll");
            }
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("GetAll");
        }
        public async Task<ActionResult> _Delete(string id)
        {
            var model = await UnitOfWorkService.FileService.GetByIdAsync(id);
            return PartialView(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(FileInfoDTO item)
        {
            var result = await UnitOfWorkService.FileService.DeleteAsync(item.Id, Server.MapPath("~"));
            if (result.Succedeed)
            {
                if (User.IsInRole("moderator"))
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToAction("GetAll");
                }
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("GetAll");
            }
            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("GetAll");
        }
    }
}