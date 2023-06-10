using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Models
{
    public class Books
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        public string Image { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Avaliable")]
        public bool IsAvaliable { get; set; }

        [Display(Name = "Book Category")]
        [Required]
        public int BookCategoryId { get; set; }
        [ForeignKey("BookCategoryId")]
        public BookCategories BookCategories { get; set; }
    }
}
