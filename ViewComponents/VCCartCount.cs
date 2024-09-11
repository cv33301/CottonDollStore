using DbCottonDollStore.Models;
using DbCottonDollStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class VCCartCount : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public VCCartCount(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            VMOrderDetails orderDetail = new VMOrderDetails()
            {
                User = await _context.User.Where(u => u.UserID == id).ToListAsync(),
                Orders = await _context.Order.Where(o => o.UserID == id && o.OrderDate == null).Include(od => od.OrderDetail).ToListAsync(),
                OrderDetails = await _context.OrderDetail.ToListAsync(),
            };

            return View(orderDetail);
        }
    }
}
