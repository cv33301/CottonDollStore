using CottonDollStore.ViewModels;
using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DbCottonDollStore.ViewComponents
{
    public class ProductAreas : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public ProductAreas(CottonDollStoreContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string? searchString)
        {
            var query = _context.ProductSpec.Include(p => p.Pro)
                                                   .Include(od => od.OrderDetail)
                                                   .Where(p => p.Pro.Statu != true)
                                                   .OrderByDescending(p => p.Pro.UpdateTime)
                                                   .Take(25);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Pro.ProName.Contains(searchString));
            }

            var productspec = query.OrderByDescending(p => p.Pro.UpdateTime)
                                  .Take(25);

            return View(await productspec.ToListAsync());
        }

    }
}
