using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App;
using App.Models;
using Microsoft.AspNetCore.Authorization;
using App.Data;

namespace m01_Start.Controllers.Blog
{
     [Area("Blog")]
     [Route("/Blog/{action=index}/{id?}")]
     [Authorize(Roles = RoleName.Administrator)]
     public class BlogController : Controller
     {
          private readonly AppDbContext _context;

          public BlogController(AppDbContext context)
          {
               _context = context;
          }

          // GET: Blog
          public async Task<IActionResult> Index()
          {
               // var appDbContext = _context.Categories.Include(c => c.ParentCategory);
               var qr = (from c in _context.Categories select c)
                        .Include(c => c.ParentCategory)
                        .Include(c => c.CategoryChildren);
               var categories = (await qr.ToListAsync())
                                .Where(c => c.ParentCategory == null)
                                .ToList();
               return View(categories);
          }

          // GET: Blog/Details/5
          public async Task<IActionResult> Details(int? id)
          {
               if (id == null || _context.Categories == null)
               {
                    return NotFound();
               }

               var category = await _context.Categories
                   .Include(c => c.ParentCategory)
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (category == null)
               {
                    return NotFound();
               }

               return View(category);
          }
          public void CreateSelectItem(List<Category> source, List<Category> des, int level)
          {
               string prefix = string.Concat(Enumerable.Repeat("----", level));
               foreach (var category in source)
               {
                    // category.Title = prefix + category.Title;
                    des.Add(new Category()
                    {
                         Id = category.Id,
                         Title = prefix + category.Title
                    });
                    if (category.CategoryChildren?.Count > 0)
                    {
                         CreateSelectItem(category.CategoryChildren.ToList(), des, level + 1);
                    }
               }
          }
          // GET: Blog/Create
          public async Task<IActionResult> Create()
          {
               var qr = (from c in _context.Categories select c)
                      .Include(c => c.ParentCategory)
                      .Include(c => c.CategoryChildren);
               var categories = (await qr.ToListAsync())
                                .Where(c => c.ParentCategory == null)
                                .ToList();
               categories.Insert(0, new Category()
               {
                    Id = -1,
                    Title = "Khong co danh muc cha"
               });

               var items = new List<Category>();
               CreateSelectItem(categories, items, 0);

               var selectList = new SelectList(items, "Id", "Title");
               ViewData["ParentCategoryId"] = selectList;
               return View();
          }

          // POST: Blog/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
          {
               if (ModelState.IsValid)
               {
                    if (category.ParentCategoryId == -1)
                    {
                         category.ParentCategoryId = null;
                    }
                    _context.Add(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
               }
               var qr = (from c in _context.Categories select c)
                       .Include(c => c.ParentCategory)
                       .Include(c => c.CategoryChildren);
               var categories = (await qr.ToListAsync())
                                .Where(c => c.ParentCategory == null)
                                .ToList();
               categories.Insert(0, new Category()
               {
                    Id = -1,
                    Title = "Khong co danh muc cha"
               });

               var items = new List<Category>();
               CreateSelectItem(categories, items, 0);

               var selectList = new SelectList(items, "Id", "Title");

               ViewData["ParentCategoryId"] = selectList;

               return View(category);
          }

          // GET: Blog/Edit/5
          public async Task<IActionResult> Edit(int? id)
          {
               if (id == null || _context.Categories == null)
               {
                    return NotFound();
               }

               var category = await _context.Categories.FindAsync(id);

               var myCategory = _context.Categories.ToList();
               myCategory.Insert(0, new Category()
               {
                    Id = -1,
                    Title = "Khong co danh muc cha"
               });
               if (category == null)
               {
                    return NotFound();
               }
               ViewData["ParentCategoryId"] = new SelectList(myCategory, "Id", "Title");
               return View(category);
          }

          // POST: Blog/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,Slug,ParentCategoryId")] Category category)
          {
               if (id != category.Id)
               {
                    return NotFound();
               }
               if(category.ParentCategoryId == category.Id){
                    ModelState.AddModelError(string.Empty,"Phai chon danh muc khac");
               }

               if (ModelState.IsValid && category.ParentCategoryId != category.Id)
               {
                    try
                    {
                         if (category.ParentCategoryId == -1)
                         {
                              category.ParentCategoryId = null;
                         }
                         _context.Update(category);
                         await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                         if (!CategoryExists(category.Id))
                         {
                              return NotFound();
                         }
                         else
                         {
                              throw;
                         }
                    }
                    return RedirectToAction(nameof(Index));
               }
               ViewData["ParentCategoryId"] = new SelectList(_context.Categories, "Id", "Title");
               return View(category);
          }

          // GET: Blog/Delete/5
          public async Task<IActionResult> Delete(int? id)
          {
               if (id == null || _context.Categories == null)
               {
                    return NotFound();
               }

               var category = await _context.Categories
                   .Include(c => c.ParentCategory)
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (category == null)
               {
                    return NotFound();
               }

               return View(category);
          }

          // POST: Blog/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
               if (_context.Categories == null)
               {
                    return Problem("Entity set 'AppDbContext.Categories'  is null.");
               }
               // var category = await _context.Categories.FindAsync(id);
               var category = await _context.Categories
                              .Include(c => c.CategoryChildren)
                              .FirstOrDefaultAsync(c => c.Id == id);
               foreach (var child in category.CategoryChildren)
               {
                    child.ParentCategoryId = category.ParentCategoryId;
               }
               if (category != null)
               {
                    _context.Categories.Remove(category);
               }

               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
          }

          private bool CategoryExists(int id)
          {
               return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
          }
     }
}
