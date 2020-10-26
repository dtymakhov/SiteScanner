using System;
using System.Collections.Generic;
using System.Diagnostics;
using HtmlAgilityPack;
using SiteScanner.Models;

namespace SiteScanner.Services
{
    public class SiteScannerService
    {
        private int _maxLinksCount;
        private string _host;
        private HashSet<string> _urls;
        private Queue<string> _queueUrls;
        
        public SiteScannerService()
        {
            _maxLinksCount = 5;
            _urls = new HashSet<string>();
            _queueUrls = new Queue<string>();
        }
        
        public List<Page> Scan(string host)
        {
            var pages = new List<Page>();
            this._host = host;
            GetUrlPerformance(host);

            while (_queueUrls.Count != 0)
            {
                var url = _queueUrls.Dequeue();
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
                return _host + url;
            return url;
        }


        private void UrlParser(HtmlDocument htmlDoc)
        {
            var nodes = htmlDoc.DocumentNode.SelectNodes("//a");
            if (nodes == null) return;
            foreach (var node in nodes)
            {
                if (_urls.Count == _maxLinksCount) return;
                
                var href = node.GetAttributeValue("href", null);

                if (href == null) continue;

                href = CorrectUrl(href);

                if (_urls.Contains(href) || !href.Contains(_host)) continue;

                _urls.Add(href);
                _queueUrls.Enqueue(href);
            }
        }
    }
}