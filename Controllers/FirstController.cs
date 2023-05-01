using Microsoft.AspNetCore.Mvc;

namespace m01_Start.Controllers
{
    public class FirstController : Controller
    {
        private IWebHostEnvironment Environment;
        private readonly ILogger<FirstController> _logger;
        private readonly ProductService _productService;
        public FirstController(ILogger<FirstController> logger, IWebHostEnvironment _environment, ProductService productService)
        {
            _logger = logger;
            Environment = _environment;
            _productService = productService;
        }
        public string Index()
        {
            _logger.LogInformation("Log information from index controller");
            _logger.LogDebug("Thong bao");
            _logger.LogCritical("Thong bao");
            return "Toi la index cua first";
        }
        public void Nothing()
        {
            _logger.LogInformation("Nothing Action");
            Response.Headers.Add("hi", "Xin chao cac ban");

        }
        public Object AnyThing()
        {
            return DateTime.Now;
        }
        public IActionResult ReadMe()
        {
            var content = @"
            Hi
            hello
            Form Ngoc Anh";
            return Content(content, "text/plain");
        }
        public IActionResult Bird()
        {
            string wwwPath = this.Environment.WebRootPath;
            string contentPath = this.Environment.ContentRootPath;
            System.Console.WriteLine("======");
            System.Console.WriteLine(contentPath);
            var imgPath = Path.Combine(contentPath, "Files", "sea.jpg");
            System.Console.WriteLine("=====");
            var bytes = System.IO.File.ReadAllBytes(imgPath);
            return File(bytes, "image/jpg");

        }
        public IActionResult Json()
        {
            return Json(
                new
                {
                    name = "Iphone",
                    price = "999"
                }
            );
        }
        public IActionResult localredirect()
        {
            var url = Url.Action("Privacy", "Home");
            return LocalRedirect(url);
        }
        public IActionResult PublicRedirect()
        {
            var url = "https://www.google.com";
            return Redirect(url);
        }
        public IActionResult MyView(string user)
        {
            if( user == null ){
                user = "Guest";

            }
            var userX = "NgocAnh";

            // return View("/Hello/HelloView.cshtml",user);
            return View((object)user);
            // return View("Hello2");
        }
        [TempData]
        public string ThongBao { get; set; }
        public IActionResult ViewProduct(int? id){
            var product = _productService.Where(p => p.Id == id).FirstOrDefault();
            ViewData["p2"] = _productService.Where(p => p.Id == 2).FirstOrDefault();
            ViewBag.p3 =  _productService.Where(p => p.Id == 2).FirstOrDefault();
            // System.Console.WriteLine(product.Id);
            if(product == null){
                ThongBao = "San pham ban yeu cau khong co";
                return Redirect(Url.Action("Index","Home"));
            }

            // return Content("San pham co id: " + id + "---" );
            return View(product);
        }
    }
}