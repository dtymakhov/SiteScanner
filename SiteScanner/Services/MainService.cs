using System.Collections.Generic;
using System.Linq;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Models;
using SiteScanner.ViewModels;

namespace SiteScanner.Services
{
    public class MainService
    {
        private ISiteRepository _siteRepository;
        private IPageRepository _pageRepository;
        private SiteScannerService _siteScannerService;

        public MainService(ISiteRepository siteRepository, IPageRepository pageRepository)
        {
            _siteRepository = siteRepository;
            _pageRepository = pageRepository;
            _siteScannerService = new SiteScannerService();
        }

        public IEnumerable<ResultPageViewModel> GetResult(string url)
        {
            var siteByUrl = _siteRepository.GetSiteByUrl(url);

            var result = _siteScannerService.Scan(url);

            return siteByUrl != null ? UpdateSite(result) : AddSite(result);
        }

        private List<ResultPageViewModel> AddSite(List<PageViewModel> pageViewModel)
        {
            var site = new Site {Url = pageViewModel[0].Url};
            var resultPages = new List<ResultPageViewModel>();

            foreach (var r in pageViewModel)
            {
                var page = new Page
                {
                    Url = r.Url,
                    MaxResponseTime = r.ResponseTime,
                    MinResponseTime = r.ResponseTime
                };

                resultPages.Add(new ResultPageViewModel
                {
                    Url = r.Url,
                    ResponseTime = r.ResponseTime,
                    MaxResponseTime = r.ResponseTime,
                    MinResponseTime = r.ResponseTime
                });

                page.Histories.Add(new History {Date = r.Date, ResponseTime = r.ResponseTime});
                site.Pages.Add(page);
            }

            _siteRepository.AddSite(site);
            _siteRepository.SaveChanges();

            return resultPages;
        }

        private List<ResultPageViewModel> UpdateSite(IEnumerable<PageViewModel> pageViewModel)
        {
            var resultPages = new List<ResultPageViewModel>();

            foreach (var p in pageViewModel)
            {
                var page = _pageRepository.GetPage(p.Url);

                if (page.MaxResponseTime < p.ResponseTime)
                    page.MaxResponseTime = p.ResponseTime;

                if (page.MinResponseTime > p.ResponseTime)
                    page.MinResponseTime = p.ResponseTime;

                resultPages.Add(new ResultPageViewModel
                {
                    Url = page.Url,
                    ResponseTime = p.ResponseTime,
                    MaxResponseTime = page.MaxResponseTime,
                    MinResponseTime = page.MinResponseTime,
                });
                _pageRepository.UpdatePage(page);
                _siteRepository.SaveChanges();
            }

            return resultPages;
        }

        public List<HistoryViewModel> GetHistory(string url)
        {
            var histories = _siteRepository.GetHistory(url);

            return histories.Select(history => new HistoryViewModel
            {
                PageUrl = history.Page.Url,
                ResponseTime = history.ResponseTime,
                Date = history.Date
            }).ToList();
        }
    }
}