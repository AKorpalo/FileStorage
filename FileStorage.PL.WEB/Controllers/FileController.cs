using System;
using System.Collections.Generic;
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
    public class FileController : LangController
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }

        private int numberOfObjectsPerPage = 2 ;
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
                        TempData["SuccessMessage"] = result.Message;
                        return RedirectToAction("Getall", "File");
                    }
                    TempData["ErrorMessage"] = result.Message;
                    return RedirectToAction("Getall", "File");
                }
            }
            return RedirectToAction("Getall", "File");
        }

        [Authorize]
        public async Task<ActionResult> GetAll()
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var pages = list.Where(p => p.UserId == User.Identity.Name).ToList().Count();
            var fileInfoDtos = list.Where(p => p.UserId == User.Identity.Name).Take(numberOfObjectsPerPage).ToList();
            var model = new FilesListViewModel()
            {
                Files = fileInfoDtos,
                Pages = pages / numberOfObjectsPerPage
            }; 
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> _GetAll(string searchString)
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var pages = list.Where(p => p.UserId == User.Identity.Name)
                            .Where(f => f.FileName.Contains(searchString))
                            .ToList().Count();
            var fileInfoDtos = list.Where(p => p.UserId == User.Identity.Name)
                            .Where(f => f.FileName.Contains(searchString))
                            .Take(numberOfObjectsPerPage).ToList();
            var serchModel = new FilesListViewModel()
            {
                Files = fileInfoDtos,
                Pages = pages / numberOfObjectsPerPage,
                SearchString = searchString
            };
            return PartialView("_Table", serchModel);
        }

        [Authorize]
        public async Task<ActionResult> _Pages(FilesListViewModel viewModel)
        {
            var model = viewModel;
            if (model.SearchString == null)
            {
                model.SearchString = "";
            }

            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var fileInfoDtos = list.Where(p => p.UserId == User.Identity.Name)
                                   .Where(f => f.FileName.Contains(model.SearchString))
                                   .Skip(numberOfObjectsPerPage * model.Pages)
                                   .Take(numberOfObjectsPerPage).ToList();
            var pages = list.Count() / numberOfObjectsPerPage;
            var serchModel = new FilesListViewModel()
            {
                Files = fileInfoDtos,
                Pages = pages,
                SearchString = model.SearchString
            };
            return PartialView("_TableBody", serchModel);
        }

        [HttpPost]
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