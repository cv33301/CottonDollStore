using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class HotRanks : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public HotRanks(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var product = _context.Product.Include(p => p.ProductSpec)
                                                .Where(p => p.Statu != true && p.ProductSpec.Any(ps => ps.Quantity > 0))
                                                .OrderByDescending(p => p.Clicks)
                                                .Take(8);
            return View(await product.ToListAsync());
        }
    }
}
