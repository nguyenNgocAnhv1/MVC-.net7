using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using m01_Start.Services;
using Microsoft.AspNetCore.Mvc;

namespace m01_Start.Controllers
{
     [Route("he-mat-troi/[action]")]
     public class PlanetController : Controller
     {
          private readonly PlanetService _planetService;

          public PlanetController(PlanetService planetService)
          {
               _planetService = planetService;
          }
          [BindProperty(SupportsGet = true, Name = "action")]
          public string actionName {get; set; }
          public IActionResult TestBindAction(){
               return Content(actionName);
          }

          public IActionResult Index()
          {
               return View();
          }
          
          
          [Route("/mec/{Name}")]  // insert / in the first letter to set absolute path
          public IActionResult Mercury(string Name)
          {
               var planet = _planetService.Where(p => p.name == Name).FirstOrDefault();
               return View("Detail", planet);
          }
          [Route("[controller]--[action].hiha",Name = "routeUrl")]
          public IActionResult Venus(string Name)
          {
               var planet = _planetService.Where(p => p.name == Name).FirstOrDefault();
               return View("Detail", planet);
          }
          public IActionResult Earth(string Name)
          {
               var planet = _planetService.Where(p => p.name == Name).FirstOrDefault();
               return View("Detail", planet);
          }
          [Route("hanhtinh/{id}")]
          public IActionResult PlanetInfor(int id)
          {
               var planet = _planetService.Where(p => p.id == id).FirstOrDefault();
               return View("Detail", planet);
          }

     }
}