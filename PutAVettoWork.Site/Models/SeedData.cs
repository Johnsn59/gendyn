using PutAVettoWork.Site.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PutAVettoWork.Site.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new GenDynContext(serviceProvider.GetRequiredService<DbContextOptions<GenDynContext>>()))
            {
                if (context.Pages.Any())
                {
                    return;
                }

                context.Pages.AddRange(
                    new Page
                    {
                        Title = "Home",
                        Slug = "home",
                        Content = "home page",
                        Sorting = 0
                    },
                    new Page
                    {
                        Title = "Who We Are",
                        Slug = "who-we-are",
                        Content = "Who We Are",
                        Sorting = 100
                    },
                    new Page
                    {
                        Title = "Events",
                        Slug = "events",
                        Content = "events page",
                        Sorting = 100
                    },
                    new Page
                    {
                        Title = "Careers",
                        Slug = "careers",
                        Content = "careers page",
                        Sorting = 100
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
