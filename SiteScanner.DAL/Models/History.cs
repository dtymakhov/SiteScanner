using System;

namespace SiteScanner.DAL.Models
{
    public class History
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int ResponseTime { get; set; }
        
        public int PageId { get; set; }
        public Page Page { get; set; }
    }
}