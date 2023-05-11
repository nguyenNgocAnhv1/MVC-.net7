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
using X.PagedList;
using Microsoft.AspNetCore.Identity;

namespace m01_Start.Controllers.Blog
{
     [Area("Blog")]
     [Route("/Post/{action=index}/{id?}")]
     [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
     public class PostController : Controller
     {

          [TempData]
          public string StatusMessage { get; set; }
          private readonly AppDbContext _context;
          private readonly UserManager<AppUser> _userManager;

          public PostController(AppDbContext context, UserManager<AppUser> userManager)
          {
               _context = context;
               _userManager = userManager;
          }






          // GET: Post
          public async Task<IActionResult> Index(int? page, int? pageSize)
          {
               if (page == null)
               {
                    page = 1;
               }
               if (pageSize == null)
               {
                    pageSize = 5;
               }
               ViewBag.stt = (page * pageSize) - 4;
               ViewBag.PageSize = pageSize;
               var appDbContext = await _context.Posts.Include(p => p.Author).Include(p => p.PostCategories).ThenInclude(pc => pc.Category).OrderByDescending(p => p.DateCreated).ToListAsync();
               return View(appDbContext.ToPagedList((int)page, (int)pageSize));
          }

          // GET: Post/Details/5
          public async Task<IActionResult> Details(int? id)
          {
               if (id == null || _context.Posts == null)
               {
                    return NotFound();
               }

               var post = await _context.Posts
                   .Include(p => p.Author)
                   .FirstOrDefaultAsync(m => m.PostId == id);
               if (post == null)
               {
                    return NotFound();
               }

               return View(post);
          }

          // GET: Post/Create
          public async Task<IActionResult> Create()
          {
               // ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
               var categories = await _context.Categories.ToListAsync();
               ViewBag.categoriesSelect = new MultiSelectList(categories, "Id", "Title");
               return View();
          }

          // POST: Post/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs")] CreatPostModels post)
          {
               var categories = await _context.Categories.ToListAsync();
               ViewBag.categoriesSelect = new MultiSelectList(categories, "Id", "Title");
               if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug))
               {
                    ModelState.AddModelError("Slug", "Insert another slug");
                    return View(post);
               }
               if (ModelState.IsValid)
               {
                    if (post.Slug == null)
                    {
                         post.Slug = App.Utilities.AppUtilities.GenerateSlug(post.Title);
                    }
                    var user = await _userManager.GetUserAsync(this.User);
                    post.DateCreated = post.DateUpdated = DateTime.Now;
                    post.AuthorId = user.Id;

                    _context.Add(post);

                    if (post.CategoryIDs != null)
                    {
                         foreach (var cateId in post.CategoryIDs)
                         {
                              _context.Add(new PostCategory()
                              {
                                   CategoryID = cateId,
                                   Post = post
                                   // PostID = post.PostId
                              });
                         }
                    }
                    await _context.SaveChangesAsync();
                    StatusMessage = "Successfully Add Post";
                    return RedirectToAction(nameof(Index));
               }
               ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
               return View(post);
          }

          // GET: Post/Edit/5
          public async Task<IActionResult> Edit(int? id)
          {
               var categories = await _context.Categories.ToListAsync();
               ViewBag.categoriesSelect = new MultiSelectList(categories, "Id", "Title");
               if (id == null || _context.Posts == null)
               {
                    return NotFound();
               }

               // var post = await _context.Posts.FindAsync(id);
               var post = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p => p.PostId == id);
               if (post == null)
               {
                    return NotFound();
               }
               var postEdit = new CreatPostModels()
               {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    Description = post.Description,
                    Slug = post.Slug,
                    Published = post.Published,
                    CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray()
               };
               return View(postEdit);
          }

          // POST: Post/Edit/5
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published,CategoryIDs")] CreatPostModels post)
          {
               var categories = await _context.Categories.ToListAsync();
               ViewBag.categoriesSelect = new MultiSelectList(categories, "Id", "Title");
               if (id != post.PostId)
               {
                    return NotFound();
               }
               if (post.Slug == null)
               {
                    post.Slug = App.Utilities.AppUtilities.GenerateSlug(post.Title);
               }
               if (await _context.Posts.AnyAsync(p => p.Slug == post.Slug && p.PostId != id))
               {
                    ModelState.AddModelError("Slug", "Insert another slug");
                    return View(post);
               }

               if (ModelState.IsValid)
               {
                    var postUpdate = await _context.Posts.Include(p => p.PostCategories).FirstOrDefaultAsync(p => p.PostId == id);
                    if (postUpdate == null)
                    {
                         return NotFound();
                    }
                    postUpdate.PostId = post.PostId;
                    postUpdate.Title = post.Title;
                    postUpdate.Content = post.Content;
                    postUpdate.Description = post.Description;
                    postUpdate.Slug = post.Slug;
                    postUpdate.Published = post.Published;
                    post.DateUpdated = DateTime.Now;
                    // postUpdate.CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray()
                    if (post.CategoryIDs == null)
                    {
                         post.CategoryIDs = new int[] { };
                    }
                    var oldCateIds = postUpdate.PostCategories.Select(c => c.CategoryID).ToArray();
                    var newCateIds = post.CategoryIDs;
                    var removeCatePosts = from postCate in postUpdate.PostCategories
                                          where (!newCateIds.Contains(postCate.CategoryID))
                                          select postCate;
                    _context.PostCategories.RemoveRange(removeCatePosts);
                    var addCateIds = from CateId in newCateIds
                                     where (!oldCateIds.Contains(CateId))
                                     select CateId;
                    foreach (var cateId in addCateIds)
                    {
                         _context.PostCategories.Add(new PostCategory()
                         {
                              PostID = id,
                              CategoryID = cateId
                         });
                    }
                    try
                    {
                         _context.Update(postUpdate);
                         await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                         if (!PostExists(post.PostId))
                         {
                              return NotFound();
                         }
                         else
                         {
                              throw;
                         }
                    }
                    StatusMessage = "Successfully Update";

                    return RedirectToAction(nameof(Index));
               }
               ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", post.AuthorId);
               return View(post);
          }

          // GET: Post/Delete/5
          public async Task<IActionResult> Delete(int? id)
          {
               if (id == null || _context.Posts == null)
               {
                    return NotFound();
               }

               var post = await _context.Posts
                   .Include(p => p.Author)
                   .FirstOrDefaultAsync(m => m.PostId == id);
               if (post == null)
               {
                    return NotFound();
               }

               return View(post);
          }

          // POST: Post/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
               if (_context.Posts == null)
               {
                    return Problem("Entity set 'AppDbContext.Posts'  is null.");
               }
               var post = await _context.Posts.FindAsync(id);
               StatusMessage = "Xoa thanh cong bai viet " + post.Title;

               if (post != null)
               {
                    _context.Posts.Remove(post);
               }

               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
          }

          private bool PostExists(int id)
          {
               return (_context.Posts?.Any(e => e.PostId == id)).GetValueOrDefault();
          }
     }
}
