using App;
using App.Data;
using App.Services;
using m01_Start;
using m01_Start.Models;
using m01_Start.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();
// config mail settings
builder.Services.AddOptions();
var mailSettings = builder.Configuration.GetSection("MailSettings");
builder.Services.Configure<MailSettings>(mailSettings);
builder.Services.AddSingleton<IEmailSender, SendMailService>();
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
builder.Services.AddSingleton<IdentityErrorDescriber, AppIdentityErrorDescriber>();

// Congif Role
builder.Services.AddAuthorization(option =>
{
     option.AddPolicy("ViewManageMenu", o => {
          o.RequireAuthenticatedUser();
          o.RequireRole(RoleName.Administrator);

     });
});

// Setup dbcontext
builder.Services.AddDbContext<AppDbContext>(options =>
        {
             string conntectString = builder.Configuration.GetConnectionString("AppMvcConnectString");
             // System.Console.WriteLine(conntectString);
             options.UseSqlServer(conntectString);
        });

// add identity
builder.Services.AddDefaultIdentity<AppUser>()
                       .AddRoles<IdentityRole>()      // add fix add role error
                       .AddEntityFrameworkStores<AppDbContext>()
                       //    .AddDefaultTokenProviders()
                       ;

// Config Identity 
builder.Services.Configure<IdentityOptions>(options =>
        {
             // Thiết lập về Password
             options.Password.RequireDigit = false; // Không bắt phải có số
             options.Password.RequireLowercase = false; // Không bắt phải có chữ thường
             options.Password.RequireNonAlphanumeric = false; // Không bắt ký tự đặc biệt
             options.Password.RequireUppercase = false; // Không bắt buộc chữ in
             options.Password.RequiredLength = 3; // Số ký tự tối thiểu của password
             options.Password.RequiredUniqueChars = 1; // Số ký tự riêng biệt
                                                       // Cấu hình Lockout - khóa user
             options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1); // Khóa 5 phút
             options.Lockout.MaxFailedAccessAttempts = 3; // Thất bại 5 lầ thì khóa
             options.Lockout.AllowedForNewUsers = true;
             // Cấu hình về User.
             options.User.AllowedUserNameCharacters = // các ký tự đặt tên user
                 "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
             options.User.RequireUniqueEmail = true;  // Email là duy nhất
                                                      // Cấu hình đăng nhập.
             options.SignIn.RequireConfirmedEmail = true;            // Cấu hình xác thực địa chỉ email (email phải tồn tại)
             options.SignIn.RequireConfirmedPhoneNumber = false;     // Xác thực số điện thoại
                                                                     // required confirm email
             options.SignIn.RequireConfirmedAccount = true;
        });
builder.Services.AddAuthentication()
           .AddGoogle(option =>
           {
                var gconfig = builder.Configuration.GetSection("Authentication:Google");
                option.ClientId = gconfig["ClientId"];
                option.ClientSecret = gconfig["ClientSecret"];
                option.CallbackPath = "/google";
           })
           .AddFacebook(option =>
           {
                var fconfig = builder.Configuration.GetSection("Authentication:Facebook");
                option.AppId = fconfig["AppId"];
                option.AppSecret = fconfig["AppSecret"];
                option.CallbackPath = "/facebook";
           })
           // .AddTwitter()
           ;

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

// configure the authentication (Must to srart identity)
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
