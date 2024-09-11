using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class ToDoLists : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public ToDoLists(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string uid)
        {
            var Order = await _context.Order.Include(o => o.OrderDetail).Where(o => o.UserID == uid ).ToListAsync();

            return View(Order);
        }
    }
}
