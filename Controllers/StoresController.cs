using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using CottonDollStore.ViewModels;
using Microsoft.Build.Evaluation;

namespace DbCottonDollStore.Controllers
{
    public class StoresController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public StoresController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Stores
        public async Task<IActionResult> Index()
        {
            var cottonDollStoreContext = _context.Store.Include(s => s.User);
            return View(await cottonDollStoreContext.ToListAsync());
        }

        // GET: Stores/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VMStoreProducts store = new VMStoreProducts()
            {
                Store = await _context.Store.Where(s => s.StoreID == id).ToListAsync(),
                User = await _context.User.Where(u => u.StoreID == id).ToListAsync(),
                Products = await _context.Product.Where(p => p.StoreID == id).ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync(),
                OrderDetail = await _context.OrderDetail.ToListAsync()

            };

            if (store == null)
            {
                ViewData["Message"] = "目前沒有訂單";
                return View("Orders", "NoOrder"/*, new { id }*/);
            }

            return View(store);
        }
        public async Task<IActionResult> Preview(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VMProductSpec spec = new VMProductSpec()
            {
                Products = await _context.Product.Where(p => p.StoreID == id).ToListAsync(),
                Store = await _context.Store.Where(s => s.StoreID == id).ToListAsync(),
                User = await _context.User.ToListAsync(),
                Category = await _context.Category.Where(c => _context.Product.Any(p => p.StoreID == id && p.CategoryID == c.CategoryID)).ToListAsync(),
                Spec = await _context.ProductSpec.Where(c => _context.Product.Any(p => p.StoreID == id && p.ProID == c.ProID)).ToListAsync()
            };

            if (spec == null)
            {
                return NotFound();
            }

            return View(spec);
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID");
            return View();
        }

        // POST: Stores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StoreID,UserID,StoreName,StoreInformation")] Store store)
        {
            if (ModelState.IsValid)
            {
                _context.Add(store);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", store.UserID);
            return View(store);
        }

        // GET: Stores/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store.Include(u => u.User).FirstOrDefaultAsync(s => s.UserID == id);
            if (store == null)
            {
                return NotFound();
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", store.UserID);
            return View(store);
        }

        // POST: Stores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("StoreID,UserID,StoreName,StoreInformation")] Store store)
        {
            if (id != store.StoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(store);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoreExists(store.StoreID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details" , new { id = store.StoreID });
            }
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", store.UserID);
            return View(store);
        }

        // GET: Stores/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var store = await _context.Store
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StoreID == id);
            if (store == null)
            {
                return NotFound();
            }

            return View(store);
        }

        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var store = await _context.Store.FindAsync(id);
            if (store != null)
            {
                _context.Store.Remove(store);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StoreExists(string id)
        {
            return _context.Store.Any(e => e.StoreID == id);
        }
    }
}
