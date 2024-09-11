using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class Banners : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public Banners(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var banner = await _context.Banner.Where(b => b.EndDate >= DateTime.Now).OrderByDescending(p => p.StartDate).ThenByDescending(p => p.BID).ToListAsync();

            return View(banner);
        }
    }
}
