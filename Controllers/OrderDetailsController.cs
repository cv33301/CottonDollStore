using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using CottonDollStore.ViewModels;
using DbCottonDollStore.ViewModels;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace DbCottonDollStore.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public OrderDetailsController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: OrderDetails
        public async Task<IActionResult> Index()
        {
            var cottonDollStoreContext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Pro);
            return View(await cottonDollStoreContext.ToListAsync());
        }

        // GET: OrderDetails/Details/5
        public async Task<IActionResult> Details(string id, string userid)
        {
            if (id == null)
            {
                return View();
            }
            VMOrderDetails orderDetail = new VMOrderDetails()
            {
                Products = await _context.Product.Where(s => _context.OrderDetail.Any(p => p.ProID == s.ProID && p.OrderID == id)).ToListAsync(),
                Spec = await _context.ProductSpec.ToListAsync(),
                Store = await _context.Store.Where(s => _context.Order.Any(p => p.StoreID == s.StoreID && p.OrderID == id)).ToListAsync(),
                User = await _context.User.ToListAsync(),
                Orders = await _context.Order.Where(p => p.OrderID == id).ToListAsync(),
                OrderDetails = await _context.OrderDetail.Where(p => p.OrderID == id).ToListAsync()
            };

            if (orderDetail == null)
            {
                return View();
            }
            ViewData["UID"] = userid;
            return View(orderDetail);
        }

        // GET: OrderDetails/Create
        public IActionResult Create()
        {
            
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID");
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID");
            ViewData["ProSpecID"] = new SelectList(_context.ProductSpec, "ProSpecID", "ProSpecID");
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID");
            return View();
        }

        // POST: OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderID,Quantity,BuyerReview,SellerRespond,Star,ProSpecID,ProID,StoreID")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID", orderDetail.ProID);
            ViewData["ProSpecID"] = new SelectList(_context.ProductSpec, "ProSpecID", "ProSpecID", orderDetail.ProSpecID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", orderDetail.StoreID);

            return View(orderDetail);
        }

        // GET: OrderDetails/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID", orderDetail.ProID);
            ViewData["ProSpecID"] = new SelectList(_context.ProductSpec, "ProSpecID", "ProSpecID", orderDetail.ProSpecID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", orderDetail.StoreID);
            return View(orderDetail);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetCartQty(string ProSpecID, string ProID, string OrderID, int qty, string operationType)
        {

            var orderDetail = await _context.OrderDetail.FindAsync(OrderID, ProSpecID, ProID);
            if (orderDetail == null)
            {
                return NotFound();
            }

            var checkQty = await _context.ProductSpec.Where(ps => ps.ProID == ProID && ps.ProSpecID == ProSpecID).FirstOrDefaultAsync();
            var uid = await _context.Order.Where(o => o.OrderID == orderDetail.OrderID).Select(o => o.UserID).FirstOrDefaultAsync();


            if (operationType == "minus")
            {
                orderDetail.Quantity += qty;
            }
            
            if (operationType == "checkqty")
            {
                orderDetail.Quantity = qty;
            }


            if (orderDetail.Quantity > checkQty.Quantity)
            {
                orderDetail.Quantity = checkQty.Quantity;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (orderDetail.Quantity <= 0)
                    {
                        var checkOD = await _context.OrderDetail.Where(o => o.OrderID == OrderID).ToListAsync();
                        var order = await _context.Order.Where(o => o.OrderID == OrderID).FirstOrDefaultAsync();
                        _context.OrderDetail.Remove(orderDetail);
                        if (checkOD.Count() == 1)
                        {
                            _context.Order.Remove(order);
                        }

                    } else 
                    { 
                        _context.Update(orderDetail); 
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("shoppingcard", "Orders", new { uid = uid });
            }

            return RedirectToAction("shoppingcard", "Orders", new { uid = uid });
        }
        // POST: OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("OrderID,Quantity,BuyerReview,SellerRespond,Star,ProSpecID,ProID,StoreID")] OrderDetail orderDetail)
        {
            if (id != orderDetail.OrderID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.OrderID))
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
            ViewData["OrderID"] = new SelectList(_context.Order, "OrderID", "OrderID", orderDetail.OrderID);
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID", orderDetail.ProID);
            ViewData["ProSpecID"] = new SelectList(_context.ProductSpec, "ProSpecID", "ProSpecID", orderDetail.ProSpecID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", orderDetail.StoreID);
            return View(orderDetail);
        }

        // GET: OrderDetails/Delete/5
        public async Task<IActionResult> Delete(string oid, string psid, string pid)
        {
            if (oid == null || psid == null || pid == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Pro)
                .Include(o => o.Spec)
                .Include(o => o.Store)
                .FirstOrDefaultAsync(m => m.OrderID == oid && m.ProSpecID == psid && m.ProID == pid);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string OrderID, string ProSpecID, string ProID)
        {
            var orderDetail = await _context.OrderDetail.FindAsync(OrderID, ProSpecID, ProID);

            var checkOD = await _context.OrderDetail.Where(o => o.OrderID == OrderID).ToListAsync();
            var order = await _context.Order.Where(o => o.OrderID == OrderID).FirstOrDefaultAsync();
            

            if (orderDetail != null)
            {
                _context.OrderDetail.Remove(orderDetail);
                if (checkOD.Count() == 1)
                {
                    _context.Order.Remove(order);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("shoppingcard", "Orders", new { uid = order.UserID });
            }

            return View(orderDetail);
        }

        private bool OrderDetailExists(string id)
        {
            return _context.OrderDetail.Any(e => e.OrderID == id);
        }

        public async Task<IActionResult> Review(string oid, string pid, string psid)
        {
            var review = _context.OrderDetail
                                             .Include(o => o.Order)
                                             .Include(o => o.Pro)
                                             .Include(o => o.Spec)
                                             .Where(_ => _.OrderID == oid && _.ProID == pid && _.ProSpecID == psid);
                                    

            return View(await review.FirstOrDefaultAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review( OrderDetail orderDetail)
        {
            var result = await _context.OrderDetail.Where(od => od.OrderID == orderDetail.OrderID && od.ProID == orderDetail.ProID && od.ProSpecID == orderDetail.ProSpecID).FirstOrDefaultAsync();

            if (result == null )
            {
                return View(orderDetail);
            }

            result.Star = orderDetail.Star;
            result.BuyerReview = orderDetail.BuyerReview;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                }
                return RedirectToAction("Details", "Products", new { id = orderDetail.ProID });
            }

            return View(orderDetail);
        }

        public async Task<IActionResult> Respond(string oid, string pid, string psid)
        {
            var review = _context.OrderDetail
                                             .Include(o => o.Order)
                                             .Include(o => o.Pro)
                                             .Include(o => o.Spec)
                                             .Where(_ => _.OrderID == oid && _.ProID == pid && _.ProSpecID == psid);


            return View(await review.FirstOrDefaultAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(OrderDetail orderDetail)
        {
            var result = await _context.OrderDetail.Where(od => od.OrderID == orderDetail.OrderID && od.ProID == orderDetail.ProID && od.ProSpecID == orderDetail.ProSpecID).FirstOrDefaultAsync();

            if (result == null)
            {
                return View(orderDetail);
            }

            result.SellerRespond = orderDetail.SellerRespond;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction("Details", "Products", new { id = orderDetail.ProID });
            }

            return View(orderDetail);
        }
        
        public async Task<IActionResult> Report(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = _context.OrderDetail
                                             .Include(o => o.Order)
                                             .Include(o => o.Pro)
                                             .Include(o => o.Spec)
                                             .Include(o => o.Store)
                                             .Where(od => od.StoreID == id);

            ViewData["Name"] = await _context.Store.Include(o => o.User)
                                                   .Where(od => od.StoreID == id)
                                                   .Select(s => s.User.Name)
                                                   .FirstOrDefaultAsync();

            return View(await review.ToListAsync());
        }
    }
}
