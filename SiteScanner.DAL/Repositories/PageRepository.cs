using System.Linq;
using SiteScanner.DAL.EF;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Models;

namespace SiteScanner.DAL.Repositories
{
    public class PageRepository : IPageRepository
    {
        private readonly ApplicationContext _applicationContext;

        public PageRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public void UpdatePage(Page page)
        {
            var updatedPage = _applicationContext.Pages.FirstOrDefault(p => p.Id == page.Id);
            
            if(updatedPage == null)
                return;

            _applicationContext.Pages.Update(updatedPage);
        }

        public Page GetPage(string url)
        {
            return _applicationContext.Pages.FirstOrDefault(p => p.Url == url);
        }
    }
}