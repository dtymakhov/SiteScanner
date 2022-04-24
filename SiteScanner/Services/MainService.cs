using System.Collections.Generic;
using System.Linq;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Models;
using SiteScanner.ViewModels;

namespace SiteScanner.Services;

public class MainService
{
    private readonly ISiteRepository _siteRepository;
    private readonly IPageRepository _pageRepository;
    private readonly SiteScannerService _siteScannerService;

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

        return siteByUrl != null ? UpdateSite(siteByUrl, result) : AddSite(result);
    }

    public List<PageViewModel> GetHistory(string url)
    {
        var histories = _siteRepository.GetHistory(url);

        return histories.Select(history => new PageViewModel
        {
            Url = history.Page.Url,
            ResponseTime = history.ResponseTime,
            Date = history.Date
        }).ToList();
    }

    public bool IsSiteAdded(string url)
    {
        var siteByUrl = _siteRepository.GetSiteByUrl(url);

        return siteByUrl != null;
    }

    private List<ResultPageViewModel> AddSite(List<PageViewModel> pageViewModel)
    {
        var site = new Site {Url = pageViewModel[0].Url};
        var resultPages = new List<ResultPageViewModel>();

        foreach (var pvm in pageViewModel)
        {
            resultPages.Add(new ResultPageViewModel
            {
                Url = pvm.Url,
                ResponseTime = pvm.ResponseTime,
                MaxResponseTime = pvm.ResponseTime,
                MinResponseTime = pvm.ResponseTime
            });
            AddPageFromPageViewModel(site, pvm);
        }

        _siteRepository.AddSite(site);
        _siteRepository.SaveChanges();

        return resultPages;
    }

    private List<ResultPageViewModel> UpdateSite(Site site, IEnumerable<PageViewModel> pageViewModel)
    {
        var resultPages = new List<ResultPageViewModel>();

        foreach (var pvm in pageViewModel)
        {
            var page = _pageRepository.GetPage(pvm.Url) ?? AddPageFromPageViewModel(site, pvm);

            if (page.MaxResponseTime < pvm.ResponseTime)
                page.MaxResponseTime = pvm.ResponseTime;

            if (page.MinResponseTime > pvm.ResponseTime)
                page.MinResponseTime = pvm.ResponseTime;

            resultPages.Add(new ResultPageViewModel
            {
                Url = page.Url,
                ResponseTime = pvm.ResponseTime,
                MaxResponseTime = page.MaxResponseTime,
                MinResponseTime = page.MinResponseTime,
            });
            _pageRepository.UpdatePage(page);
            _siteRepository.SaveChanges();
        }

        return resultPages;
    }

    private Page AddPageFromPageViewModel(Site site, PageViewModel pageViewModel)
    {
        var page = new Page
        {
            Url = pageViewModel.Url,
            MaxResponseTime = pageViewModel.ResponseTime,
            MinResponseTime = pageViewModel.ResponseTime
        };
        page.Histories.Add(new History
        {
            Date = pageViewModel.Date,
            ResponseTime = pageViewModel.ResponseTime
        });
            
        site.Pages.Add(page);
            
        return page;
    }
}