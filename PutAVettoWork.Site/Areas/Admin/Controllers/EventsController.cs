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
        public async Task<IActionResult> Create(EventPost eventPost)
            //Here is where I realized I messed up naming this event *Fixed
        {
            if (ModelState.IsValid)
            {
                eventPost.Slug = eventPost.Name.ToLower().Replace(" ", "-");

                var slug = await context.EventPosts.FirstOrDefaultAsync(x => x.Slug == eventPost.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "This event alreay exists");
                    return View(eventPost);
                }

                context.Add(eventPost);
                await context.SaveChangesAsync();

                TempData["Success"] = "The event has been added!";


                return RedirectToAction("Index");
            }
            return View(eventPost);
        }

        //GET /admin/events/details/id
        public async Task<IActionResult> Details(int id)
        {
            EventPost eventPost = await context.EventPosts.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (eventPost == null)
            {
                return NotFound();
            }
            return View(eventPost);
        }

        //GET /admin/events/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            EventPost eventPost = await context.EventPosts.FindAsync(id);
            if (eventPost == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", eventPost.CategoryId);
            return View(eventPost);
        }

        //POST /admin/pages/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, EventPost eventPost)
        //Here is where I realized I messed up naming this event *Fixed
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", eventPost.CategoryId);

            if (ModelState.IsValid)
            {
                eventPost.Slug = eventPost.Name.ToLower().Replace(" ", "-");

                var slug = await context.EventPosts.Where(x => x.Id != Id).FirstOrDefaultAsync(x => x.Slug == eventPost.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "This event alreay exists");
                    return View(eventPost);
                }

                context.Update(eventPost);
                await context.SaveChangesAsync();

                TempData["Success"] = "The event has been updated!";


                return RedirectToAction("Index");
            }
            return View(eventPost);
        }

        //GET /admin/Events/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            EventPost eventPost = await context.EventPosts.FindAsync(id);
            if (eventPost == null)
            {
                TempData["Error"] = "This event does not exist!";
            }
            else
            {
                context.EventPosts.Remove(eventPost);
                await context.SaveChangesAsync();

                TempData["Success"] = "The event has been deleted!";
            }
            return RedirectToAction("Index");
        }

    }
}