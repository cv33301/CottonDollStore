using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using DbCottonDollStore.ViewModels;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DbCottonDollStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public OrdersController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cottonDollStoreContext = await _context.Order.Where(o => o.UserID == id && o.OrderDate != null)
                                                       .Include(o => o.Store)
                                                       .Include(o => o.User)
                                                       .ToListAsync();

            if (cottonDollStoreContext == null)
            {
                return View();
            }

            ViewData["UID"] = id;
            ViewData["Name"] = _context.User.Find(id).Name;
            return View(cottonDollStoreContext);
        }
        public async Task<IActionResult> SellerOrders(string sid, string uid)
        {
            var sellerOrders = await _context.Order.Where(o => o.StoreID == sid && o.OrderDate != null)
                                             .Include(o => o.Store)
                                             .Include(o => o.User)
                                             .OrderBy(o => o.OrderDate)
                                             .ToListAsync();

            if (sellerOrders == null)
            {
                return View();
            }

            ViewData["SID"] = sid;
            ViewData["UID"] = uid;
            ViewData["Name"] = _context.User.Find(uid).Name;
            return View(sellerOrders);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(string id, string oid)
        {
            if (oid == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Store)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == oid);


            if (order == null)
            {
                return NotFound();
            }
            ViewData["UID"] = id;
            return View(order);
        }

        public async Task<IActionResult> Shoppingcard(string uid)
        {
            VMOrderDetails shoppingcard = new VMOrderDetails()
            {
                Orders = await _context.Order.Where(o => o.UserID == uid).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(od => _context.Order.Any(o => o.OrderID == od.OrderID && o.UserID == uid)).ToListAsync(),
                Products = await _context.Product.ToListAsync(),
                Store = await _context.Store.Where(s => _context.Order.Any(o => o.StoreID == s.StoreID && o.UserID == uid)).ToListAsync(),
                User = await _context.User.Where(u => _context.Order.Any(o => o.UserID == u.UserID && o.UserID == uid)).ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync()
            };

            if (shoppingcard.OrderDetails == null)
            {
                ViewData["Message"] = "目前沒有訂單";
                return View("NoOrder"/*, new { id }*/);
            }

            ViewData["UID"] = uid;
            return View(shoppingcard);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, OrderDetail orderDetail)
        {
            if (order.UserID == null)
            {
                TempData["error"] = "請登入會員";
                return RedirectToAction("Details", "Products", new { id = orderDetail.ProID });
            }

            var result = await _context.Order
                    .FromSqlRaw("SELECT dbo.getOrderID() AS OrderID")
                    .Select(o => o.OrderID)
                    .FirstOrDefaultAsync();

            var checkuser = await _context.Store.Where(s => s.StoreID == order.StoreID).FirstOrDefaultAsync();

            if(order.UserID == checkuser.UserID)
            {
                TempData["error"] = "不可下單自己的商品";
                return RedirectToAction("Details", "Products", new { id = orderDetail.ProID });
            }

            var checkOid = await _context.Order
                    .Where(o => o.UserID == order.UserID && o.OrderDate == null && o.StoreID == order.StoreID )
                    .FirstOrDefaultAsync();

            order.OrderID = result;

            if (ModelState.IsValid)
            {
                //如果訂單存在
                if (checkOid != null)
                {
                    order.OrderID = checkOid.OrderID;
                    orderDetail.OrderID = order.OrderID;

                    var checkPid = await _context.OrderDetail
                            .Where(od => od.OrderID == checkOid.OrderID && od.ProID == orderDetail.ProID && od.ProSpecID == orderDetail.ProSpecID)
                            .FirstOrDefaultAsync();

                    //如果產品存在
                    if (checkPid != null)
                    {
                        var specQty = await _context.ProductSpec.Where(ps => ps.ProID == checkPid.ProID && ps.ProSpecID == checkPid.ProSpecID).FirstOrDefaultAsync();

                        checkPid.Quantity += orderDetail.Quantity;

                        if (checkPid.Quantity > specQty.Quantity)
                        {
                            checkPid.Quantity = specQty.Quantity;
                        }
                        _context.Update(checkPid);
                        await _context.SaveChangesAsync();
                    } else
                    {
                        _context.Add(orderDetail);
                        await _context.SaveChangesAsync();
                    }

                }

                //訂單不存在
                if (checkOid == null)
                {

                    _context.Add(order);

                    orderDetail.OrderID = order.OrderID;
                    _context.Add(orderDetail);
                    await _context.SaveChangesAsync();
                }
                

                return RedirectToAction("Shoppingcard", new { uid = order.UserID });
            }

            return View(orderDetail);
        }

        public async Task<IActionResult> checkout(string id)
        {
            VMOrderDetails shoppingcard = new VMOrderDetails()
            {
                Orders = await _context.Order.Where(o => o.OrderID == id).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(od => od.OrderID == id).ToListAsync(),
                Products = await _context.Product.ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync(),
                Store = await _context.Store.Where(s => _context.Order.Any(o => o.OrderID == id && o.StoreID == s.StoreID)).ToListAsync(),
                User = await _context.User.Where(u => _context.Order.Any(o => o.OrderID == id && o.UserID == u.UserID)).ToListAsync(),
            };

            if (shoppingcard.OrderDetails == null)
            {
                return NotFound();
            }

            //ViewData["UID"] = uid;
            return View(shoppingcard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(string Logistics,string Payment, string OrderID)
        {
            var result = await _context.Order.Where(o => o.OrderID == OrderID).Include(od => od.OrderDetail).FirstOrDefaultAsync();
            if (OrderID == null)
            {
                return NotFound();
            }
            if (result == null)
            {
                return NotFound();
            }

            result.OrderDate = DateTime.Now;
            result.Logistics = Logistics;
            result.Payment = Payment;

            if (ModelState.IsValid)
            {
                foreach (var item in result.OrderDetail)
                {
                    var PS = await _context.ProductSpec.Where(ps => ps.ProID == item.ProID && ps.ProSpecID == item.ProSpecID).FirstOrDefaultAsync();
                    PS.Quantity -= item.Quantity;
                    if(PS.Quantity == 0)
                    {
                        var p = await _context.Product.FindAsync(item.ProID);
                        p.Statu = true;
                        _context.Update(p);
                    }
                    _context.Update(PS);
                }

                _context.Update(result);
                await _context.SaveChangesAsync();
                return RedirectToAction("Completed", new { id = OrderID });
            }

            return View(Payment);

        }

        public async Task<IActionResult> Completed(string id)
        {
            VMOrderDetails shoppingcard = new VMOrderDetails()
            {
                Orders = await _context.Order.Where(o => o.OrderID == id).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(od => od.OrderID == id).ToListAsync(),
                Products = await _context.Product.ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync(),
                Store = await _context.Store.Where(s => _context.Order.Any(o => o.OrderID == id && o.StoreID == s.StoreID)).ToListAsync(),
                User = await _context.User.Where(u => _context.Order.Any(o => o.OrderID == id && o.UserID == u.UserID)).ToListAsync(),
            };

            if (shoppingcard.OrderDetails == null)
            {
                return NotFound();
            }

            return View(shoppingcard);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", order.StoreID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,StoreID,UserID,OrderDate,Payment,Logistics,ConNumber,PreDate,ShipDate,DeliveryDate,PickupDate,Statu")] Order order)
        {
            if (id != order.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
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
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", order.StoreID);
            ViewData["UserID"] = new SelectList(_context.User, "UserID", "UserID", order.UserID);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Store)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(string id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pickup(string OrderID)
        {
            var order = await _context.Order.FindAsync(OrderID);

            if (order == null)
            {
                return NotFound();
            }

            order.ConNumber = "00000000";
            order.PreDate = DateTime.Now;
            order.ShipDate = DateTime.Now;
            order.DeliveryDate = DateTime.Now;
            order.PickupDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "OrderDetails", new { id = order.OrderID, userid = order.UserID } );
            }

            return View(order);
        }
    }
}
