using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SiteScanner.DAL.EF;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Models;

namespace SiteScanner.DAL.Repositories
{
    public class SiteRepository : ISiteRepository
    {
        private ApplicationContext _applicationContext;

        public SiteRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public void AddSite(Site site)
        {
            _applicationContext.Add(site);
        }

        public Site GetSiteByUrl(string url)
        {
            return _applicationContext.Sites.FirstOrDefault(p => p.Url.Equals(url));
        }

        public IEnumerable<History> GetHistory(string url)
        {
            return _applicationContext.Histories
                .Include(p => p.Page)
                .Include(p => p.Page.Site)
                .Where(p => p.Page.Site.Url.Equals(url))
                .AsNoTracking()
                .ToList();
        }

        public void SaveChangesAsync()
        {
            _applicationContext.SaveChangesAsync();
        }
    }
}