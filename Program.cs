using App;
using m01_Start;
using m01_Start.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
// config view url
builder.Services.Configure<RazorViewEngineOptions>(option =>
{
     // View/controller/cshtml => // hello/controller/cshtml
     // 0 - action
     // 1 - controller
     // 2 - area

     option.ViewLocationFormats.Add("/Hello/{1}/{0}" + RazorViewEngine.ViewExtension);
});
// add services
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<PlanetService>();



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


// config error 400 - 599
// app.UseStatusCodePages(
//     appError => {
//         appError.Run(async context => {
//             var response =  context.Response;
//             var code = response.StatusCode;
//             var content = @$"<html>
//             <head>
//                 <meta charset='UTF-8'/>
//                 <title>Error {code}</title>

//             </head>
//                 <body style='color: red; font-size:24px'>
//                     There was an error {code} - {(HttpStatusCode)code}
//                 </body>
//             </html>";
//             await response.WriteAsync(content);
//         });
//     }
// );
app.AddStatusCodePage();

app.MapGet("/sayhi", async (context) =>
{
     await context.Response.WriteAsync($"Hello Asp.net MVC {DateTime.Now}");
});

// config map controller
// app.MapControllerRoute(
//     name: "FirstRoute",
//     // pattern: "{RanDomString?}/{controller=Home}/{action=Index}/{id:range(2,3)?}" ,// config parameters route
//     // defaults: new{
//     //     controller= "First",
//     //     action = "ViewProduct",
//     //     id = 2, // default parameter route
//     // }
//     constraints: new{
//         // RanDomString = new StringRouteConstraint("abc"), // constraint url 
//         // id = new RangeRouteConstraint(1,2)
//     }
// );

//config area
app.MapAreaControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}",
    areaName: "ProductManage"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
