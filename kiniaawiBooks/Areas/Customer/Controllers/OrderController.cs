using kiniaawiBooks.Data;
using kiniaawiBooks.Models;
using kiniaawiBooks.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private ApplicationDbContext _db;

        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        //get checkout action method
        public IActionResult Checkout()
        {
            return View();
        }

        //post checkout action method
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Checkout(Order anOrder)
        {
            List<Books> books = HttpContext.Session.Get<List<Books>>("books");
            if(books!=null)
            {
                foreach (var book in books)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.BookId = book.Id;
                    //anOrder.OrderDetails = new List<OrderDetails>();
                    anOrder.OrderDetails.Add(orderDetails);
                }
            }
            anOrder.OrderNo = GetOrderNo();
            _db.Orders.Add(anOrder);
            await _db.SaveChangesAsync();
            HttpContext.Session.Set("books", new List<Books>());
            return View();
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
