using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SiteScanner.DAL.Models
{
    public class Page
    {
        public Page()
        {
            Histories = new List<History>();
        }
        [Key]
        public int Id { get; set; }
        public string Url { get; set; }
        public int MaxResponseTime { get; set; }
        public int MinResponseTime { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
        public List<History> Histories { get; set; }
    }
}