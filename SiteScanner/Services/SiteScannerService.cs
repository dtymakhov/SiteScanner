using System;
using System.Collections.Generic;
using System.Diagnostics;
using HtmlAgilityPack;
using SiteScanner.Models;

namespace SiteScanner.Services
{
    public class SiteScannerService
    {
        private int maxLinksCount;
        private string host;
        private HashSet<string> urls;
        private Queue<string> queueUrls;
        
        public SiteScannerService()
        {
            maxLinksCount = 5;
            urls = new HashSet<string>();
            queueUrls = new Queue<string>();
        }
        
        public List<Page> Scan(string host)
        {
            var pages = new List<Page>();
            this.host = host;
            GetUrlPerformance(host);

            while (queueUrls.Count != 0)
            {
                var url = queueUrls.Dequeue();
                var urlPerformance = (int) GetUrlPerformance(url);
                pages.Add(new Page {Url = url, ResponseTime = urlPerformance});
            }

            return pages;
        }


        private long GetUrlPerformance(string url)
        {
            var web = new HtmlWeb();
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var htmlDoc = web.Load(url);
            stopwatch.Stop();

            UrlParser(htmlDoc);
            return stopwatch.ElapsedMilliseconds;
        }

        private string CorrectUrl(string url)
        {
            if (url.Contains("?"))
                return url.Substring(0, url.IndexOf('?'));
            if (url.StartsWith('/'))
                return host + url;
            return url;
        }


        private void UrlParser(HtmlDocument htmlDoc)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//a");
            if (nodes == null) return;
            foreach (var node in nodes)
            {
                if (urls.Count == maxLinksCount) return;
                
                var href = node.GetAttributeValue("href", null);

                if (href == null) continue;

                href = CorrectUrl(href);

                if (urls.Contains(href) || !href.Contains(host)) continue;

                urls.Add(href);
                queueUrls.Enqueue(href);
            }
        }
    }
}