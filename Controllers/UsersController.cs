using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using Microsoft.Data.SqlClient;
using CottonDollStore.ViewModels;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace DbCottonDollStore.Controllers
{
    public class UsersController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public UsersController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var cottonDollStoreContext = _context.User.Include(u => u.Rank);
            return View(await cottonDollStoreContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Rank)
                .Include(u => u.Store)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserPhone,Password,UserImg,Name,Birthday,Gender,Email,Account,RankID,RegDate,StoreID")] User user)
        {
            var checkEmail = await _context.User.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            var checkPhone = await _context.User.Where(u => u.UserPhone == user.UserPhone).FirstOrDefaultAsync();
            if (checkEmail != null)
            {
                ViewData["Message_Email"] = "此信箱已註冊過!";
                return View(user);
            }
            if (checkPhone != null)
            {
                ViewData["Message_Phone"] = "此手機已註冊過!";
                return View(user);
            }
            var result = await _context.User
                    .FromSqlRaw("SELECT dbo.getUserID() AS UserID")
                    .Select(u => u.UserID)
                    .FirstOrDefaultAsync();

            user.UserID = result;
            user.UserImg = null;
            user.Account = null;
            user.RankID = "1";
            user.RegDate = DateTime.Now;
            user.StoreID = null;
            _context.Add(user);
            if (ModelState.IsValid)
            {
                await _context.SaveChangesAsync();

                HttpContext.Session.SetString("Manager", user.UserID);
                return RedirectToAction("Details", new { id = user.UserID});
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string Name, IFormFile? photo)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (photo != null && photo.Length > 0)
            {
                if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View(user);
                }
                //string fileName = Path.GetFileName(photo.FileName);
                // 确定上传目录的路径為 wwwroot/Photos ，並存成uploadPath
                user.UserImg = user.UserID + Path.GetExtension(photo.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "user", user.UserImg);

                // 使用文件流將文件上傳至伺服器
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                ViewData["Message"] = "上傳成功!";

            }

            user.Name = Name;

            if (ModelState.IsValid)
            {

                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new { id = user.UserID });
            }
            return View(user);
        }

        public async Task<IActionResult> EditPW(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPW(string id, string Password)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {

                try
                {
                    user.Password = Password;
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                TempData["SuccessMessage"] = "更新成功！"; 
                return RedirectToAction("EditPW", new { id = user.UserID });
            }

            return View(user);
        }

        public async Task<IActionResult> BecomeSeller(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BecomeSeller(string id, string Account)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _context.Store
                    .FromSqlRaw("SELECT dbo.getStoreID() AS StoreID")
                    .Select(u => u.StoreID)
                    .FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {

                try
                {
                    user.Account = Account;
                    user.RankID = "2";
                    user.StoreID = result;

                    var store = new Store
                    {
                        StoreID = result,
                        StoreName = null,
                        StoreInformation = null,
                        UserID = user.UserID
                    };
                    _context.Update(user);
                    _context.Add(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction( "Edit", "Stores" , new { id = user.UserID });
            }

            return View(user);
        }


        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .Include(u => u.Rank)
                .FirstOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.User.Any(e => e.UserID == id);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            if (Email == null || Password == null)
            {
                return View();
            }

            var result = await _context.User.Where(m => m.Email == Email && m.Password == Password).FirstOrDefaultAsync();

            if (result == null)
            {
                ViewData["ErrorMessage"] = "帳號密碼錯誤";
                return View();
            }

            string uid = result.UserID;
            if (result.RankID == "0")
            {
                //HttpContext.Session.SetString("Manager", JsonConvert.SerializeObject(result));
                HttpContext.Session.SetString("Manager", uid);
                return RedirectToAction("Index", "Banners");
            }
            
            HttpContext.Session.SetString("Manager", uid);
            return RedirectToAction("Index", "Home" , new { id = result.UserID });
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Manager");
            return RedirectToAction("Index", "Home");
        }
    }
}
