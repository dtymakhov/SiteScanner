using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Models;
using SiteScanner.Services;
using Page = SiteScanner.Models.Page;

namespace SiteScanner.Controllers
{
    public class HomeController : Controller
    {
        private SiteScannerService _siteScannerService;

        public HomeController()
        {
            _siteScannerService = new SiteScannerService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult<List<Page>> Result(string url)
        {
            var result = _siteScannerService.Scan(url);
            return View(result);
        }
    }
}