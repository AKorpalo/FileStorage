using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FileStorage.BLL.Interfaces;
using Ninject;

namespace FileStorage.PL.WEB.Controllers
{
    public class HomeController : Controller
    {
        [Inject]
        public IUnitOfWorkService UnitOfWorkService { get; set; }
        public async Task<ActionResult> Index()
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var model = list.Where(p => p.IsPrivate == false);
            return View(model);
        }
        public async Task<PartialViewResult> _Update(string search)
        {
            var list = await UnitOfWorkService.FileService.GetAllAsync();
            var model = list.Where(p => p.IsPrivate == false);
            var serchModel = model.Where(f => f.FileName.Contains(search));
            return PartialView("_TableBody", serchModel);
        }
    }
}