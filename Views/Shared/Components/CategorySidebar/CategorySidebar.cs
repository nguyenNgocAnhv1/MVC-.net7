using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace m01_Start.Components
{
     [ViewComponent]
     public class CategorySidebar : ViewComponent
     {
          public List<Category> Categories { get; set; }
          public int level { get; set; }
          public string caregorySlug { get; set; }
          public IViewComponentResult Invoke(CategorySidebar data)
          {
               
               return View(data);
          }
     }
}