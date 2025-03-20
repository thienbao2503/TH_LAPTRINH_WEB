using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using TH_LAP_TRINH_WEB.Models;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "category",
    pattern: "{controller=category}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "product",
    pattern: "{controller=product}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
