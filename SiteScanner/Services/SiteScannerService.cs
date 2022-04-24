using System;
using System.Collections.Generic;
using System.Diagnostics;
using HtmlAgilityPack;
using SiteScanner.ViewModels;

namespace SiteScanner.Services;

public class SiteScannerService
{
    private readonly int _maxLinksCount;
    private string _host;
    private readonly HashSet<string> _urls;
    private readonly Queue<string> _queueUrls;

    public SiteScannerService()
    {
        _maxLinksCount = 50;
        _urls = new HashSet<string>();
        _queueUrls = new Queue<string>();
    }

    public List<PageViewModel> Scan(string host)
    {
        var pages = new List<PageViewModel>();
        _host = host;
        GetUrlPerformance(_host);

        while (_queueUrls.Count != 0)
        {
            var url = _queueUrls.Dequeue();
            var urlPerformance = (int) GetUrlPerformance(url);
            pages.Add(new PageViewModel {Url = url, ResponseTime = urlPerformance, Date = DateTime.Now});
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
        if (url.Contains('?'))
            return url[..url.IndexOf('?')];

        if (url.StartsWith('/'))
            return _host + url.Remove(0, 1);

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