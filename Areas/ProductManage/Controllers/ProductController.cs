using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using m01_Start.Services;
using Microsoft.AspNetCore.Mvc;

namespace m01_Start.Controllers
{
     // /Areas/AreasName/Views/ControllerName/ActionName
     [Area("ProductManage")]
     public class ProductController : Controller
     {
          private readonly ProductService _productService;

          public ProductController(ProductService productService)
          {
               _productService = productService;
          }
          [Route("/Cac-san-pham")]
          public IActionResult Index()
          {
               abc.highlight("hhh");
               
               var product = _productService.OrderBy(p => p.Name).ToList();
               return View((object)product);
          }

     }
}