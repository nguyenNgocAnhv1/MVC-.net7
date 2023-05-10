using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
using App.Data;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace m01_Start.Controllers
{
     [Area("Database")]
     // [Route("/db-manage/[action=index]",Defaults = new { action = "Index" })]
     [Route("/db-manage/{action=index}")]
     public class DbManageController : Controller
     {
          private readonly AppDbContext _dbContext;
          [TempData]
          public string _ThongBao { get; set; }
          private readonly UserManager<AppUser> _userManager;
          private readonly RoleManager<IdentityRole> _roleManager;

          public DbManageController(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
          {
               _dbContext = dbContext;
               _userManager = userManager;
               _roleManager = roleManager;
          }

          public IActionResult Index()
          {
               return View();
          }
          [HttpGet]
          public IActionResult DeleteDb()
          {
               return View();
          }

          [HttpPost]
          public async Task<IActionResult> DeleteDbAsync()
          {
               var success = await _dbContext.Database.EnsureDeletedAsync();
               _ThongBao = success ? "Xoa db thanh cong" : "Xoa db khong thanh cong";
               return RedirectToAction(nameof(Index));
          }

          [HttpPost]
          public async Task<IActionResult> Migrate()
          {
               await _dbContext.Database.MigrateAsync();
               _ThongBao = "Cap nhat db thanh cong";
               return RedirectToAction(nameof(Index));
          }
          public async Task<IActionResult> SeedDataAsync()
          {
               var roleNames = typeof(RoleName).GetFields().ToList();
               foreach (var r in roleNames)
               {
                    var RoleName = (string)r.GetRawConstantValue();
                    var rfound = await _roleManager.FindByNameAsync(RoleName);
                    if (rfound == null)
                    {
                         await _roleManager.CreateAsync(new IdentityRole(RoleName));
                    }
               }
               var userAdmin = await _userManager.FindByNameAsync("admin");
               if (userAdmin == null)
               {
                    userAdmin = new AppUser()
                    {
                         UserName = "admin",
                         Email = "admin@example.com",
                         EmailConfirmed = true,

                    };

               }
               await _userManager.CreateAsync(userAdmin, "admin123");
               await _userManager.AddToRoleAsync(userAdmin, RoleName.Administrator);
               _ThongBao = "Create a new Role and Admin User";
               return RedirectToAction("Index");
          }
     }
}