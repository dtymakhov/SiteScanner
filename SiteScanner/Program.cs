using System.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SiteScanner.DAL.EF;
using SiteScanner.DAL.Interfaces;
using SiteScanner.DAL.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddScoped<ISiteRepository, SiteRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlite
    (builder.Configuration.GetConnectionString("SiteScannerConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();