using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteScanner.DAL.Models
{
    public class Site
    {
        public Site()
        {
            Pages = new List<Page>();
        }
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public List<Page> Pages { get; set; }
    }
}