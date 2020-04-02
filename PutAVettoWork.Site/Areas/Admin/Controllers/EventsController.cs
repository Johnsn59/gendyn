using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PutAVettoWork.Site.Data;

namespace PutAVettoWork.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventsController : Controller
    {

        private readonly GenDynContext context;

        public 
            EventsController(GenDynContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}