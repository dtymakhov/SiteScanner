using SiteScanner.DAL.Models;

namespace SiteScanner.DAL.Interfaces;

public interface IPageRepository
{
    void UpdatePage(Page page);
    Page GetPage(string url);

}