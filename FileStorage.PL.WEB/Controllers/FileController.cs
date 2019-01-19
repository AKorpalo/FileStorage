using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Microsoft.AspNet.Identity;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class FileController : Controller
    {
        [Inject]
        public IFileService FileService { get; set; }

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
                    FileDTO fileDto = new FileDTO()
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

                    var result = await FileService.Create(fileDto);

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
            var list = await FileService.GetAllAsync();
            var model = list.Where(p => p.UserId == User.Identity.Name);
            return View(model);
        }

        [Authorize]
        public async Task<PartialViewResult> _GetAll(string search)
        {
            var list = await FileService.GetAllAsync();
            var model = list.Where(p => p.UserId == User.Identity.Name);
            var serchModel = model.Where(f => f.FileName.Contains(search));
            return PartialView("_TableBody", serchModel);
        }

        public ActionResult Download(string id)
        {
            var fileDownloadInfo = FileService.CheckDownlod(id, Server.MapPath("~"));
            if (fileDownloadInfo.IsDownload.Succedeed)
            {
                string fullPath = Server.MapPath(fileDownloadInfo.Path);

                string fileType = MimeMapping.GetMimeMapping(fileDownloadInfo.FileName);

                string fileName = fileDownloadInfo.FileName;

                return File(fullPath, fileType, fileName);
            }
            return RedirectToAction("GetAll", "File");
        }

        public ActionResult Edit(string id)
        {
            var file = FileService.GetById(id);
            FileStateViewModel model = new FileStateViewModel
            {
                Id = file.Id,
                IsPrivate = file.IsPrivate
            }; 
            return View(model);
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
            var result = await FileService.UpdateAsync(file);
            if (result.Succedeed)
            {
               // ModelState.AddModelError(result.Property, result.Message);
                return RedirectToAction("GetAll");
            }
            ModelState.AddModelError(result.Property, result.Message);
            return View(model);
        }

        public ActionResult Delete(string id)
        {
            var model = FileService.GetById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(FileInfoDTO item)
        {
            var result = await FileService.DeleteAsync(item.Id, Server.MapPath("~"));
            if (result.Succedeed)
            {
                return RedirectToAction("GetAll");
            }

            return RedirectToAction("Delete", (object)item.Id);
        }
    }
}