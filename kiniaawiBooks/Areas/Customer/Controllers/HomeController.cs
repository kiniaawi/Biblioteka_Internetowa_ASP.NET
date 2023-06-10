using kiniaawiBooks.Data;
using kiniaawiBooks.Models;
using kiniaawiBooks.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.Books.Include(c=>c.BookCategories).ToList());
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //get product details action method
        //[Authorize]
        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var book = _db.Books.Include(c => c.BookCategories).FirstOrDefault(c => c.Id == id);
            if(book==null)
            {
                return NotFound();
            }
            return View(book);
        }

        //post product details action method
        [HttpPost]
        //[Authorize]
        [ActionName("Details")]
        public ActionResult BookDetails(int? id)
        {
            List<Books> books = new List<Books>();
            if (id == null)
            {
                return NotFound();
            }
            var book = _db.Books.Include(c => c.BookCategories).FirstOrDefault(c => c.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            books = HttpContext.Session.Get<List<Books>>("books");
            if(books==null)
            {
                books = new List<Books>();
            }
            books.Add(book);
            HttpContext.Session.Set("books", books);
            return View(book);
        }

        //get remove action method
        //[Authorize]
        [ActionName("Remove")]
            public IActionResult RemoveFromCart(int? id)
        {
            List<Books> books = HttpContext.Session.Get<List<Books>>("books");
            if (books != null)
            {
                var book = books.FirstOrDefault(c => c.Id == id);
                if (book != null)
                {
                    books.Remove(book);
                    HttpContext.Session.Set("books", books);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPatch]
        //[Authorize]
        public IActionResult Remove(int? id)
        {
            List<Books> books = HttpContext.Session.Get<List<Books>>("books");
            if(books!=null)
            {
                var book = books.FirstOrDefault(c => c.Id == id);
                if(book!=null)
                {
                    books.Remove(book);
                    HttpContext.Session.Set("books", books);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        //get book cart action method
        //[Authorize]
        public IActionResult Cart()
        {
            List<Books> books = HttpContext.Session.Get<List<Books>>("books");
            if(books==null)
            {
                books = new List<Books>();
            }
            return View(books);
        }

    }
}
