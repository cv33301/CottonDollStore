using CottonDollStore.ViewModels;
using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class VCProductDetails : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public VCProductDetails(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            VMProductSpec product = new VMProductSpec()
            {
                Products = await _context.Product.Where(p => p.ProID == id).ToListAsync(),
                Store = await _context.Store.Where(s => _context.Product.Any(p => p.StoreID == s.StoreID && p.ProID == id)).ToListAsync(),
                User = await _context.User.ToListAsync(),
                Category = await _context.Category.Where(c => _context.Product.Any(p => p.ProID == id && p.CategoryID == c.CategoryID)).ToListAsync(),
                CategoryID_2Navigation = await _context.Category.Where(c => _context.Product.Any(p => p.ProID == id && p.CategoryID_2 == c.CategoryID)).ToListAsync(),
                CategoryID_3Navigation = await _context.Category.Where(c => _context.Product.Any(p => p.ProID == id && p.CategoryID_3 == c.CategoryID)).ToListAsync(),
                Spec = await _context.ProductSpec.Where(p => p.ProID == id).ToListAsync()
            };

            return View(product);
        }
    }
}
