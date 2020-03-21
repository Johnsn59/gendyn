using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PutAVettoWork.Site.Data;
using PutAVettoWork.Site.Models;

namespace PutAVettoWork.Site.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly GenDynContext context;

        public JobsController(GenDynContext context)
        {
            this.context = context;
        }

        // GET /jobs
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var jobs = context.Jobs.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Jobs.Count() / pageSize);

            return View(await jobs.ToListAsync());
        }

        // GET /jobs/category
        public async Task<IActionResult> JobsByCategory(string categorySlug, int p = 1)
        {
            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if (category == null) return RedirectToAction("Index");

            int pageSize = 6;
            var jobs = context.Jobs.OrderByDescending(x => x.Id)
                                            .Where(x => x.CategoryId == category.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Jobs.Where(x => x.CategoryId == category.Id).Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            ViewBag.CategorySlug = categorySlug;

            return View(await jobs.ToListAsync());
        }
    }
}