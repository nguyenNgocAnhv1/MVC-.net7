using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using m01_Start.Services;
using App;
using App.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace m01_Start.Controllers.Blog
{
     [Area("Blog")]
     // [Route("/ViewPost/{action=index}")]
     public class ViewPostController : Controller
     {
          private readonly AppDbContext _context;

          public ViewPostController(AppDbContext context)
          {
               _context = context;
          }
          [Route("/ViewPost/{categorySlug?}")]
          public IActionResult Index(string categorySlug, int? page, int? pageSize)
          {
               // return Content(categorySlug);

               var categories = GetCategories();
               ViewBag.categories = categories;
               ViewBag.categorySlug = categorySlug;

               Category category = null;
               if (!string.IsNullOrEmpty(categorySlug))
               {
                    category = _context.Categories.Where(c => c.Slug == categorySlug)
                                                  .Include(c => c.CategoryChildren)
                                                  .FirstOrDefault();
                    if (category == null)
                    {
                         return NotFound("Khong thay category not found");
                    }
               }
               var posts = _context.Posts.Include(p => p.Author)
                                         .Include(p => p.PostCategories)
                                         .ThenInclude(p => p.Category)
                                         .AsQueryable();
               posts.OrderByDescending(p => p.DateUpdated);
               if (category != null)
               {
                    var ids = new List<int>();
                    category.ChildCategoryId(null, ids);
                    ids.Add(category.Id);
                    posts = posts.Where(p => p.PostCategories.Where(pc => ids.Contains(pc.CategoryID)).Any());
               }
               ViewBag.category = category;
               // config pagelist
               if (page == null)
               {
                    page = 1;
               }
               if (pageSize == null)
               {
                    pageSize = 10;
               }
               ViewBag.stt = (page * pageSize) - 4;
               ViewBag.PageSize = pageSize;
             
               return View(posts.ToPagedList((int)page, (int)pageSize));
          }
          [Route("/ViewPost/{postSlug}.html")]
          public IActionResult Detail(string postSlug)
          {
                var categories = GetCategories();
               ViewBag.categories = categories;

               var post = _context.Posts.Where( p => p.Slug == postSlug)
                                        .Include( p => p.Author)
                                        .Include( p => p.PostCategories)
                                        .ThenInclude(p => p.Category)
                                        .FirstOrDefault();
               if(post == null){
                    return NotFound();
               }
               var category = post.PostCategories.FirstOrDefault()?.Category;
               ViewBag.category = category;
               var otherPost = _context.Posts.Where(p => p.PostCategories.Any( c => c.Category.Id == category.Id))
                                             .Where(p => p.PostId != post.PostId)
                                             .OrderByDescending(p => p.DateUpdated)
                                             .Take(5);
               ViewBag.otherPost = otherPost;
               return View(post);

          }
          public List<Category> GetCategories()
          {
               var categories = _context.Categories
                                .Include(c => c.CategoryChildren)
                                .AsEnumerable()
                                .Where(c => c.ParentCategory == null)
                                .ToList();

               return categories;
          }
     }
}