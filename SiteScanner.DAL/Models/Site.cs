using System.Collections.Generic;

namespace SiteScanner.DAL.Models
{
    public class Site
    {
        public int Id { get; set; }
        public string Url { get; set; }
        
        public List<Page> Pages { get; set; }
    }
}