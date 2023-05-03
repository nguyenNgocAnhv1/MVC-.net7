using App;
using m01_Start;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
// config view url
builder.Services.Configure<RazorViewEngineOptions>(option => {
    // View/controller/cshtml => // hello/controller/cshtml
    // 0 - action
    // 1 - controller
    // 2 - area
   
    option.ViewLocationFormats.Add("/Hello/{1}/{0}"+ RazorViewEngine.ViewExtension );
});
// add services
builder.Services.AddSingleton<ProductService>();

// Setup dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
        {
             string conntectString = builder.Configuration.GetConnectionString("AppMvcConnectString");
             // System.Console.WriteLine(conntectString);
             options.UseSqlServer(conntectString);
        });
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
