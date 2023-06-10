using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Models
{
    public class OrderDetails
    {
        public int ID { get; set; }

        [Display(Name="Order")]
        public int OrderId { get; set; }

        [Display(Name ="Book")]
        public int BookId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        [ForeignKey("BookId")]
        public Books Book { get; set; }
    }
}
