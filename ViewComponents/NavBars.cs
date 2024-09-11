using Microsoft.AspNetCore.Mvc;
using DbCottonDollStore.Models;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class NavBars : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public NavBars(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            var User = await _context.User.FirstOrDefaultAsync(u => u.UserID == id);

            return View(User);
        }
    }
}
