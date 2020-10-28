using SiteScanner.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace SiteScanner.DAL.EF
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        
        public DbSet<History> Histories { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Site> Sites { get; set; }
    }
}