using System;
using System.ComponentModel.DataAnnotations;

namespace SiteScanner.DAL.Models;

public class History
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int ResponseTime { get; set; }
        
    public int PageId { get; set; }
    public Page Page { get; set; }
}