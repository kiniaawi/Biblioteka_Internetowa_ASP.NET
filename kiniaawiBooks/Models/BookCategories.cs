using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Models
{
    public class BookCategories
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Book Category")]
        public string BookCategory { get; set; }

    }
}
