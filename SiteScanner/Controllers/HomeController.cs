using System.Collections.Generic;
using System.Linq;
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
            var correctedUrl = SiteCheckerService.CorrectHost(url);
            if (!SiteCheckerService.IsWebSiteOnline(correctedUrl))
                return Content("Please enter a correct url");
            

            var result = _mainService.GetResult(correctedUrl)
                .OrderByDescending(r => r.ResponseTime);

            return View(result);
        }

        [HttpPost]
        public ActionResult<IEnumerable<PageViewModel>> History(string url)
        {
            var correctedUrl = SiteCheckerService.CorrectHost(url);

            if (!_mainService.IsSiteAdded(correctedUrl)) 
                return Content("This site has never been checked");
            
            var history = _mainService.GetHistory(correctedUrl);
            return View(history);

        }
    }
}