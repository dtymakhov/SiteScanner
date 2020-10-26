using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SiteScanner.Models;
using SiteScanner.Services;

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