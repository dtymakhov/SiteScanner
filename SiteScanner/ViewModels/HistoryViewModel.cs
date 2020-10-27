using System;

namespace SiteScanner.ViewModels
{
    public class HistoryViewModel
    {
        public string PageUrl { get; set; }
        public int ResponseTime { get; set; }
        public DateTime Date { get; set; }
    }
}