using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;

namespace DbCottonDollStore.Controllers
{
    public class RanksController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public RanksController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Ranks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Rank.ToListAsync());
        }

        // GET: Ranks/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Rank
                .FirstOrDefaultAsync(m => m.RankID == id);
            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // GET: Ranks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Ranks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RankID,RankName")] Rank rank)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rank);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rank);
        }

        // GET: Ranks/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Rank.FindAsync(id);
            if (rank == null)
            {
                return NotFound();
            }
            return View(rank);
        }

        // POST: Ranks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("RankID,RankName")] Rank rank)
        {
            if (id != rank.RankID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rank);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RankExists(rank.RankID))
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
            return View(rank);
        }

        // GET: Ranks/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rank = await _context.Rank
                .FirstOrDefaultAsync(m => m.RankID == id);
            if (rank == null)
            {
                return NotFound();
            }

            return View(rank);
        }

        // POST: Ranks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var rank = await _context.Rank.FindAsync(id);
            if (rank != null)
            {
                _context.Rank.Remove(rank);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RankExists(string id)
        {
            return _context.Rank.Any(e => e.RankID == id);
        }
    }
}
