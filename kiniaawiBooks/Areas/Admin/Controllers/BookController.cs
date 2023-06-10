using kiniaawiBooks.Data;
using kiniaawiBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private ApplicationDbContext _db;
        private IHostingEnvironment _he;

        public BookController(ApplicationDbContext db, IHostingEnvironment he)
        {
            _db = db;
            _he = he;
        }
        public IActionResult Index()
        {
            return View(_db.Books.Include(c=>c.BookCategories).ToList());
        }

        //post index action method
        [HttpPost]
        public IActionResult Index(decimal? lowAmount, decimal? largeAmount)
        {
            var books = _db.Books.Include(c => c.BookCategories).Where(c => c.Price >= lowAmount && c.Price <= largeAmount).ToList();
            if(lowAmount==null || largeAmount==null)
            {
                books = _db.Books.Include(c => c.BookCategories).ToList();
            }
            return View(books);
        }

        //Get Create method
        [Authorize]
        public IActionResult Create()
        {
            ViewData["BookCategoryId"]=new SelectList(_db.BookCategories.ToList(),"Id","BookCategory");
            return View();
        }

        //Post Create method
        [HttpPost]
        public async Task<IActionResult> Create(Books books, IFormFile image)
        {
            if(ModelState.IsValid)
            {
                var searchBook = _db.Books.FirstOrDefault(c => c.Title == books.Title);
                if(searchBook!=null)
                {
                    ViewBag.message = "This product already exists";
                    return View();
                }
                if(image!=null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    books.Image = "images/" + image.FileName;
                }

                if(image==null)
                {
                    books.Image = "images/noimage.png";
                }
                _db.Books.Add(books);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(books);

        }

        //get edit action method
        [Authorize]
        public ActionResult Edit(int? id)
        {
            ViewData["BookCategoryId"] = new SelectList(_db.BookCategories.ToList(), "Id", "BookCategory");
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

        //post edit action method
        [HttpPost]
        public async Task<IActionResult> Edit(Books books, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    var name = Path.Combine(_he.WebRootPath + "/images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name, FileMode.Create));
                    books.Image = "images/" + image.FileName;
                }

                if (image == null)
                {
                    books.Image = "images/noimage.png";
                }
                _db.Books.Update(books);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(books);
        }

        //get details action method
        [Authorize]
        public ActionResult Details(int? id)
        {
            if(id==null)
            {
                return NotFound();
                
            }
            var book = _db.Books.Include(c => c.BookCategories).FirstOrDefault(c => c.Id == id);
            if (book==null)
            {
                return NotFound();
            }
            return View(book);
        }

        //get delete action method
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var book = _db.Books.Include(c=>c.BookCategories).Where(c => c.Id == id).FirstOrDefault();

            if(book==null)
            {
                return NotFound();
            }
            return View(book);
        }

        //post delete action method
        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var book = _db.Books.FirstOrDefault(c => c.Id == id);
            if(book==null)
            {
                return NotFound();
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
