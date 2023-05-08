using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using App;
using m01_Start.Models;
using ContactModel = m01_Start.Models.Contact;
using Microsoft.AspNetCore.Authorization;
using m01_Start.Services;
using App.Data;

namespace m01_Start.Controllers.Contact
{
     [Area("Contact")]
     [Route("/Contact/{action=index}")]
     [Authorize(Roles = RoleName.Administrator)]
     public class ContactController : Controller
     {

          private readonly AppDbContext _context;
          [TempData]
          public string _ThongBao { get; set; }
          public ContactController(AppDbContext context)
          {
               _context = context;
          }

          // GET: Contact
          [HttpGet("/admin/contact")]
          public async Task<IActionResult> Index()
          {
               return _context.Contacts != null ?
                           View(await _context.Contacts.ToListAsync()) :
                           Problem("Entity set 'AppDbContext.Contacts'  is null.");
          }

          // GET: Contact/Details/5

          public async Task<IActionResult> Details(int? id)
          {
               if (id == null || _context.Contacts == null)
               {
                    return NotFound();
               }

               var contact = await _context.Contacts
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (contact == null)
               {
                    return NotFound();
               }

               return View(contact);
          }

          // GET: Contact/Create
          [AllowAnonymous]
          public IActionResult SendContact()
          {
               return View();
          }

          // POST: Contact/Create
          // To protect from overposting attacks, enable the specific properties you want to bind to.
          // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
          [HttpPost]
          [ValidateAntiForgeryToken]
          [AllowAnonymous]
          public async Task<IActionResult> SendContact([Bind("FullName,Email,DateSent,Message,Phone")] ContactModel contact)
          {
               if (ModelState.IsValid)
               {
                    _ThongBao = "Success to send contact";
                    abc.highlight(_ThongBao);
                    _context.Add(contact);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                    // return RedirectToAction("Index", "Home");

               }
               return View();
          }

          // GET: Contact/Edit/5

          public async Task<IActionResult> Delete(int? id)
          {
               if (id == null || _context.Contacts == null)
               {
                    return NotFound();
               }

               var contact = await _context.Contacts
                   .FirstOrDefaultAsync(m => m.Id == id);
               if (contact == null)
               {
                    return NotFound();
               }

               return View(contact);
          }

          // POST: Contact/Delete/5
          [HttpPost, ActionName("Delete")]
          [ValidateAntiForgeryToken]
          public async Task<IActionResult> DeleteConfirmed(int id)
          {
               if (_context.Contacts == null)
               {
                    return Problem("Entity set 'AppDbContext.Contacts'  is null.");
               }
               var contact = await _context.Contacts.FindAsync(id);
               if (contact != null)
               {
                    _context.Contacts.Remove(contact);
               }

               await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
          }


     }
}
