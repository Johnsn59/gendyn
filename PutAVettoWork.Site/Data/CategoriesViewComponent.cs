using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PutAVettoWork.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PutAVettoWork.Site.Data
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly GenDynContext context;

        public CategoriesViewComponent(GenDynContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            return context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
