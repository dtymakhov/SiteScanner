using System;
using HtmlAgilityPack;

namespace SiteScanner.Services
{
    public static class SiteCheckerService
    {
        public static string CorrectHost(string host)
        {
            if (!host.StartsWith("https://") || !host.StartsWith("https://"))
                host = "https://" + host;

            return host.StartsWith('/') ? host.Remove(0, 1) : host;
        }

        public static bool IsWebSiteOnline(string url)
        {
            try
            {
                var web = new HtmlWeb();
                web.Load(url);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
    }
}