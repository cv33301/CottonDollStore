using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using DbCottonDollStore.ViewComponents;

namespace DbCottonDollStore.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public CategoriesController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index(string id = "00")
        {   

            var cottonDollStoreContext = _context.Category.Include(c => c.ParentCategoryNavigation)
                                                          .Skip(1)
                                                          .Where(c => c.ParentCategory == id)
                                                          .OrderBy(c => c.ParentCategory)
                                                          .ThenBy(c => c.CategoryID);
            
            return View(await cottonDollStoreContext.ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategoryNavigation)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            ViewData["ParentCategory"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory == "00"), "CategoryID", "CategoryName");
            ViewData["newID"] = _context.Category.Count();
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName,ParentCategory")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategory"] = new SelectList(_context.Category, "CategoryID", "CategoryName", category.ParentCategory);
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["ParentCategory"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory == "00"), "CategoryID", "CategoryName", category.ParentCategory);
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryID,CategoryName,ParentCategory")] Category category)
        {
            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentCategory"] = new SelectList(_context.Category, "CategoryID", "CategoryName", category.ParentCategory);
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .Include(c => c.ParentCategoryNavigation)
                .FirstOrDefaultAsync(m => m.CategoryID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }

        public IActionResult GetCategoryByViewComponent(string id)
        {
            return ViewComponent("VCCategory", new { id });
        }
    }
}
