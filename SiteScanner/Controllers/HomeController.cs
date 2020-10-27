using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SiteScanner.DAL.Interfaces;
using SiteScanner.Services;
using SiteScanner.ViewModels;

namespace SiteScanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly MainService _mainService;

        public HomeController(ISiteRepository siteRepository, IPageRepository pageRepository)
        {
            _mainService = new MainService(siteRepository, pageRepository);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<IEnumerable<PageViewModel>> Result(string url)
        {
            var result = _mainService.GetResult(url);
            return View(result);
        }

        [HttpPost]
        public ActionResult<IEnumerable<HistoryViewModel>> History(string url)
        {
            var history = _mainService.GetHistory(url);
            return View(history);
        }
    }
}