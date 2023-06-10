using kiniaawiBooks.Controllers;
using kiniaawiBooks.Data;
using kiniaawiBooks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kiniaawiBooks.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> _userManager;
        ApplicationDbContext _db;
        public UserController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ApplicationUser user)
        {
            if(ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                    //TempData["create"]="User created succesfully";
                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            
            
            return View();
        }

        public async Task<IActionResult>Edit(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c=>c.Id == id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if(userInfo==null)
            {
                return NotFound();
            }
            userInfo.FirstName = user.FirstName;
            userInfo.LastName = user.LastName;
            var result = await _userManager.UpdateAsync(userInfo);
            if (result.Succeeded)
            {
                TempData["update"] = "User updated succesfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult>Delete(string id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = DateTime.Now.AddYears(100);
            int rowAffected=_db.SaveChanges();
            if(rowAffected>0)
            {
                TempData["lockout"]="User lockouted succesfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult>Active(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Active(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            userInfo.LockoutEnd = DateTime.Now.AddDays(-1);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["active"] = "User activated succesfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        public async Task<IActionResult> DeletePerm(string id)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(c => c.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePerm(ApplicationUser user)
        {
            var userInfo = _db.ApplicationUsers.FirstOrDefault(c => c.Id == user.Id);
            if (userInfo == null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(userInfo);
            int rowAffected = _db.SaveChanges();
            if (rowAffected > 0)
            {
                TempData["delete"] = "User deleted succesfully";
                return RedirectToAction(nameof(Index));
            }
            return View(userInfo);
        }

        ////REGISTER
        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Register")]
        public async Task<IActionResult> Register(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                if (result.Succeeded)
                {
                    var isSaveRole = await _userManager.AddToRoleAsync(user, "User");
                    TempData["create"] = "User created succesfully";
                    //return Redirect("Customer/Home/Index"); 
                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }


            return View();
        }

        //public async Task<IActionResult> RegisterSuccess(string returnUrl = null)
        //{
        //    //await _signInManager.SignOutAsync();
        //    HttpContext.Session.SetString("roleName", "");
            
        //    if (returnUrl != null)
        //    {
        //        return LocalRedirect(returnUrl);
        //    }
        //    else
        //    {
        //        return View();
        //    }
        //}
    }
}
