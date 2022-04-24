using System.Collections.Generic;
using SiteScanner.DAL.Models;

namespace SiteScanner.DAL.Interfaces;

public interface ISiteRepository
{
    void AddSite(Site site);
    Site GetSiteByUrl(string url);
    IEnumerable<History>  GetHistory(string url);
    void SaveChanges();
}