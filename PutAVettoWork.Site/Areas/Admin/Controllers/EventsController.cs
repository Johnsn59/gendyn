using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PutAVettoWork.Site.Data;
using PutAVettoWork.Site.Models;

namespace PutAVettoWork.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventsController : Controller
    {

        private readonly GenDynContext context;


        public EventsController(GenDynContext context)
        {
            this.context = context;
        }

        //GET /admin/events
        public async Task<IActionResult> Index()
        {

            return View(await context.EventPosts.OrderByDescending(x => x.Id).ToListAsync());
        }

        //GET /admin/events/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

        //POST /admin/pages/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventPost page)
            //Here is where I realized I messed up naming this event
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Name.ToLower().Replace(" ", "-");

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The page alreay exists");
                    return View(page);
                }

                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added!";


                return RedirectToAction("Index");
            }
            return View(page);
        }

        //GET /admin/events/details/id
        public async Task<IActionResult> Details(int id)
        {
            EventPost page = await context.EventPosts.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);
        }

    }
}