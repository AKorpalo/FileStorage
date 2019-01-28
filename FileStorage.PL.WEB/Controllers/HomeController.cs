using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FileStorage.BLL.DTO;
using FileStorage.BLL.Interfaces;
using FileStorage.PL.WEB.Models;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class HomeController : LangController
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }

        private int _numberOfObjectsPerPage = 5;

        public async Task<ActionResult> Index()
        {
            IEnumerable<FileInfoDTO> list;
            try
            {
                list = await UnitOfWorkService.FileService.GetAllAsync();
            }
            catch
            {
                return View("Error");
            }
            var fileInfoList = list.Where(p => p.IsPrivate == false).ToList();

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
            var model = new TableViewModel<FileInfoDTO>()
            {
                Items = fileInfoDtos,
                Pages = pages
            };
            return View(model);
        }
        public async Task<ActionResult> _Search(string searchString)
        {
            IEnumerable<FileInfoDTO> list;
            try
            {
                list = await UnitOfWorkService.FileService.GetAllAsync();
            }
            catch
            {
                return View("Error");
            }
            var fileInfoList = list.Where(p => p.IsPrivate == false)
                .Where(f => f.FileName.ToLower().Contains(searchString.ToLower())).ToList();

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
            var serchModel = new TableViewModel<FileInfoDTO>()
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

            IEnumerable<FileInfoDTO> list;
            try
            {
                list = await UnitOfWorkService.FileService.GetAllAsync();
            }
            catch
            {
                return View("Error");
            }
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
            var fileInfoDtos = fileInfoList.Where(p => p.IsPrivate == false)
                                           .Where(f => f.FileName.ToLower().Contains(model.SearchString.ToLower()))
                                           .Skip(_numberOfObjectsPerPage * model.Pages)
                                           .Take(_numberOfObjectsPerPage).ToList();

            var serchModel = new TableViewModel<FileInfoDTO>()
            {
                Items = fileInfoDtos,
                Pages = pages,
                SearchString = model.SearchString
            };
            return PartialView("_TableBody", serchModel);
        }
    }
}