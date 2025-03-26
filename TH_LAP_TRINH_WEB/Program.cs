using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TH_LAP_TRINH_WEB.Interface;
using TH_LAP_TRINH_WEB.Repositories;
using TH_LAP_TRINH_WEB.Models;
using TH_LAP_TRINH_WEB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddDefaultTokenProviders().AddDefaultUI().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.LogoutPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddRazorPages();

builder.Services.AddScoped<IProductRepository, EFProductRepository>();

builder.Services.AddScoped<ICategoryRepository, EFCategoryRepository>();

// Add these lines
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

// Add this line before UseAuthorization
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages();

// Định tuyến cho Areas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// Định tuyến mặc định cho ứng dụng
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
