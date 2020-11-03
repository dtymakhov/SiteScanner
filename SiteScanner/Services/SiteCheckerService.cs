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
            
            if (host.Contains("www."))
            {
                host = host.Replace("www.", "");
            }
            
            return host.EndsWith('/') ? host: host + "/";
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