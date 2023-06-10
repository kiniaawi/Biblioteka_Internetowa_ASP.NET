using kiniaawiBooks.Data;
using kiniaawiBooks.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class BookCategoriesController : Controller
    {
        private ApplicationDbContext _db;

        public BookCategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Index()
        {
            return View(_db.BookCategories.ToList());
        }

        //Create Get action Method
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        //Create Post Action Method

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult>Create(BookCategories bookCategories)
        {
            if(ModelState.IsValid)
            {
                _db.BookCategories.Add(bookCategories);
                await _db.SaveChangesAsync();
                TempData["create"] = "Category added successfully";
                return RedirectToAction(nameof(Index));
            }


            return View(bookCategories);
        }

        //Edit Get action Method
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            var bookCategory = _db.BookCategories.Find(id);
            if(bookCategory == null)
            {
                return NotFound();
            }
            return View(bookCategory);
        }

        //Edit Post action Method

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Edit(BookCategories bookCategories)
        {
            if (ModelState.IsValid)
            {
                _db.Update(bookCategories);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(bookCategories);
        }

        //Details Get action Method
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCategory = _db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            return View(bookCategory);
        }

        //Details Post action Method

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Details(BookCategories bookCategories)
        {
            return RedirectToAction(nameof(Index));

        }

        //Delete Get action Method
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookCategory = _db.BookCategories.Find(id);
            if (bookCategory == null)
            {
                return NotFound();
            }
            return View(bookCategory);
        }

        //Delete Post action Method

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete(int? id, BookCategories bookCategories)
        {
            if(id==null)
            {
                return NotFound();
            }

            if(id!=bookCategories.Id)
            {
                return NotFound();
            }

            var bookCategory = _db.BookCategories.Find(id);
            if(bookCategories==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(bookCategory);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return View(bookCategories);
        }
    }
}
