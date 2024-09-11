using DbCottonDollStore.Models;
using DbCottonDollStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class VCOrders : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public VCOrders(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            VMOrderDetails orderDetail = new VMOrderDetails()
            {
                Products = await _context.Product.Where(s => _context.OrderDetail.Any(p => p.ProID == s.ProID && p.OrderID == id)).ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync(),
                Store = await _context.Store.Where(s => _context.Order.Any(p => p.StoreID == s.StoreID && p.OrderID == id)).ToListAsync(),
                User = await _context.User.ToListAsync(),
                Orders = await _context.Order.Where(p => p.OrderID == id).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(p => p.OrderID == id).ToListAsync()
            };

            return View(orderDetail);
        }
    }
}
