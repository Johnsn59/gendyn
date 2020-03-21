using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PutAVettoWork.Site.Data;
using PutAVettoWork.Site.Models;

namespace PutAVettoWork.Site.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class JobsController : Controller
    {
        private readonly GenDynContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public JobsController(GenDynContext context, IWebHostEnvironment webHostEnvironment)
        {
            this.context = context;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET /admin/jobs
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var jobs = context.Jobs.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Jobs.Count() / pageSize);

            return View(await jobs.ToListAsync());
        }

        // GET /admin/jobs/details/id
        public async Task<IActionResult> Details(int id)
        {
            Job job = await context.Jobs.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET /admin/jobs/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

        // POST /admin/jobs/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                job.Slug = job.Name.ToLower().Replace(" ", "-");

                var slug = await context.Jobs.FirstOrDefaultAsync(x => x.Slug == job.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The job already exists.");
                    return View(job);
                }

                string imageName = "noimage.png";
                if (job.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/job_images");
                    imageName = Guid.NewGuid().ToString() + "_" + job.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await job.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                }

                job.Image = imageName;

                context.Add(job);
                await context.SaveChangesAsync();

                TempData["Success"] = "The job has been added!";

                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET /admin/jobs/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Job job = await context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", job.CategoryId);

            return View(job);
        }

        // POST /admin/jobs/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Job job)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", job.CategoryId);

            if (ModelState.IsValid)
            {
                job.Slug = job.Name.ToLower().Replace(" ", "-");

                var slug = await context.Jobs.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == job.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The job already exists.");
                    return View(job);
                }

                if (job.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/job_images");

                    if (!string.Equals(job.Image, "noimage.png"))
                    {
                        string oldImagePath = Path.Combine(uploadsDir, job.Image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + job.ImageUpload.FileName;
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await job.ImageUpload.CopyToAsync(fs);
                    fs.Close();
                    job.Image = imageName;
                }

                context.Update(job);
                await context.SaveChangesAsync();

                TempData["Success"] = "The job has been edited!";

                return RedirectToAction("Index");
            }

            return View(job);
        }

        // GET /admin/jobs/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Job job = await context.Jobs.FindAsync(id);

            if (job == null)
            {
                TempData["Error"] = "The job does not exist!";
            }
            else
            {
                if (!string.Equals(job.Image, "noimage.png"))
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/job_images");
                    string oldImagePath = Path.Combine(uploadsDir, job.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                context.Jobs.Remove(job);
                await context.SaveChangesAsync();

                TempData["Success"] = "The job has been deleted!";
            }

            return RedirectToAction("Index");
        }
    }
}