using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App;
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

          public DbManageController(AppDbContext dbContext)
          {
               _dbContext = dbContext;
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
     }
}