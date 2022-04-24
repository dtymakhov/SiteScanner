using System;

namespace SiteScanner.ViewModels;

public class PageViewModel
{
    public string Url { get; set; }
    public int ResponseTime { get; set; }
    public DateTime Date { get; set; }
}