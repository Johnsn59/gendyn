using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PutAVettoWork.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PutAVettoWork.Site.Data
{
    public class MainViewComponent : ViewComponent
    { 
        private readonly GenDynContext context;

    public MainViewComponent(GenDynContext context)
    {
        this.context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var pages = await GetPagesAsync();
        return View(pages);
    }

    private Task<List<Page>> GetPagesAsync()
    {
        return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
    }
}
}
