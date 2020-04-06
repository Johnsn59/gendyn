using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PutAVettoWork.Site.Models
{
    public class EventPost
    {
        public int Id { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum of 2 characters")]
        public string Name { get; set; }

        public string Slug { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum of 2 characters")]
        public string Desciption { get; set; }

        [Required, MinLength(2, ErrorMessage = "Minimum of 2 characters")]
        public string Content { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please choose a category!")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
    }
}
