using DbCottonDollStore.Models;
using DbCottonDollStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class RateStars : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public RateStars(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            VMOrderDetails orderDetail = new VMOrderDetails()
            {
                Products = await _context.Product.Where(s => s.ProID == id).ToListAsync(),
                Spec = await _context.ProductSpec.Where(ps => ps.ProID == id).ToListAsync(),
                Store = await _context.Store.Where(s => _context.Product.Any(p => p.StoreID == s.StoreID && p.ProID == id)).ToListAsync(),
                User = await _context.User.ToListAsync(),
                Orders = await _context.Order.Where(o => _context.OrderDetail.Any(od => od.OrderID == o.OrderID && od.ProID == id)).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(p => p.ProID == id).ToListAsync()
            };

            return View(orderDetail);
        }
    }
}
