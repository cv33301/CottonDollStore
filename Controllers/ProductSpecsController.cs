using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DbCottonDollStore.Controllers
{
    public class ProductSpecsController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public ProductSpecsController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: ProductSpecs
        public async Task<IActionResult> Index()
        {
            var cottonDollStoreContext = _context.ProductSpec.Include(p => p.Pro).Include(p => p.Store);
            return View(await cottonDollStoreContext.ToListAsync());
        }

        public async Task<IActionResult> pIndex(string id = "01")
        {
            var AreaID01 =  _context.Category
                                           .Where(p => p.CategoryID == "01" || p.ParentCategory == "01")
                                           .Select(p => p.CategoryID)
                                           .ToList();
            var AreaID05 = _context.Category
                                           .Where(p => p.CategoryID == "05" || p.ParentCategory == "05")
                                           .Select(p => p.CategoryID)
                                           .ToList();

            if (AreaID01.Contains(id))
            {
                var Area01 = _context.Product
                                         .Include(p => p.ProductSpec)
                                         .Include(p => p.Category)
                                         .Where(p => p.Statu != true &&
                                                    (p.CategoryID_2 == id || p.CategoryID_2Navigation.ParentCategory == id))
                                         .OrderByDescending(p => p.UpdateTime)
                                         .Take(25);

                return View(await Area01.ToListAsync());
            }

            if (AreaID05.Contains(id))
            {
                var Area05 = _context.Product
                                         .Include(p => p.ProductSpec)
                                         .Include(p => p.Category)
                                         .Where(p => p.Statu != true &&
                                                    (p.CategoryID_3 == id || p.CategoryID_3Navigation.ParentCategory == id))
                                         .OrderByDescending(p => p.UpdateTime)
                                         .Take(25);

                return View(await Area05.ToListAsync());
            }

            var productArea = _context.Product.Include(p => p.ProductSpec)
                                                    .Include(p => p.Category)
                                                    .Where(p => p.Statu != true && 
                                                        (p.CategoryID == id || p.Category.ParentCategory == id))
                                                    .OrderByDescending(p => p.UpdateTime)
                                                    .Take(25);                                  

            return View(await productArea.ToListAsync());
        }

        public async Task<IActionResult> sIndex(string? searchString)
        {


            var query = _context.Product.Include(p => p.ProductSpec)
                                                   .Include(od => od.OrderDetail)
                                                   .Where(p => p.Statu != true)
                                                   .OrderByDescending(p => p.UpdateTime)
                                                   .Take(25);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.ProName.Contains(searchString));
            }

            var productspec = query.OrderByDescending(p => p.UpdateTime)
                                  .Take(25);

            return View(await productspec.ToListAsync());

        }

        // GET: ProductSpecs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSpec = await _context.ProductSpec
                .Include(p => p.Pro)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProSpecID == id);
            if (productSpec == null)
            {
                return NotFound();
            }

            return View(productSpec);
        }

        // GET: ProductSpecs/Create
        public IActionResult Create(string id, string sid)
        {
            if (id == null || sid == null)
            {
                return NotFound();
            }

            ViewData["pid"] = id;
            ViewData["sid"] = sid;
            return View();
        }

        // POST: ProductSpecs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSpec productSpec, IFormFile photo)
        {
            var pid = _context.Product
                            .FromSqlRaw("SELECT dbo.getProID() AS ProID")
                            .Select(p => p.ProID)
                            .AsEnumerable()
                            .FirstOrDefault();

            productSpec.ProID = pid;

            var psid = _context.ProductSpec
                            .FromSqlRaw("SELECT dbo.getProSpecID({0}) AS ProSpecID", productSpec.ProID)
                            .Select(p => p.ProSpecID)
                            .AsEnumerable()
                            .FirstOrDefault();

            productSpec.ProSpecID = psid;


            if (photo != null && photo.Length > 0)
            {
                if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View(productSpec);
                }
                // 确定上传目录的路径為 wwwroot/Photos ，並存成uploadPath
                productSpec.SpecImg = productSpec.ProID + "_" + productSpec.ProSpecID + Path.GetExtension(photo.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "prot", productSpec.SpecImg);

                // 使用文件流將文件上傳至伺服器
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                ViewData["Message"] = "上傳成功!";

            }

            if (ModelState.IsValid)
            {
                _context.Add(productSpec);
                await _context.SaveChangesAsync();

                //return RedirectToAction("Create", "Products", new
                //{
                //    id = productSpec.StoreID,
                //    spec = productSpec.Spec, // 添加其他属性
                //    quantity = productSpec.Quantity,
                //    price = productSpec.Price
                //});

                return Json(productSpec);
            }

            return Json(productSpec);
        }

        // GET: ProductSpecs/Edit/5
        public async Task<IActionResult> Edit(string id, string pid, string storeid)
        {
            if (id == null || pid == null || storeid == null)
            {
                return NotFound();
            }

            var productSpec = await _context.ProductSpec.FindAsync(id, pid, storeid);
            if (productSpec == null)
            {
                return NotFound();
            }
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID", productSpec.ProID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", productSpec.StoreID); 

            return View(productSpec);
        }

        // POST: ProductSpecs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, string pid, string storeid, [Bind("ProSpecID,ProID,StoreID,SpecImg,Spec,Quantity,Price")] ProductSpec productSpec)
        {
            if (id != productSpec.ProSpecID || pid != productSpec.ProID || storeid != productSpec.StoreID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSpec);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSpecExists(productSpec.ProSpecID))
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
            ViewData["ProID"] = new SelectList(_context.Product, "ProID", "ProID", productSpec.ProID);
            ViewData["StoreID"] = new SelectList(_context.Store, "StoreID", "StoreID", productSpec.StoreID);
            return View(productSpec);
        }

        // GET: ProductSpecs/Delete/5
        public async Task<IActionResult> Delete(string psid, string pid, string sid)
        {
            if (psid == null || pid == null || sid == null)
            {
                return View("Index");
            }

            var productSpec = await _context.ProductSpec
                .Include(p => p.Pro)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProSpecID == psid && m.ProID == pid && m.StoreID == sid);
            if (productSpec == null)
            {
                return NotFound();
            }

            return View(productSpec);
        }

        // POST: ProductSpecs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string ProSpecID, string ProID, string StoreID)
        {
            var productSpec = await _context.ProductSpec.FindAsync(ProSpecID, ProID, StoreID);
            if (productSpec != null)
            {
                _context.ProductSpec.Remove(productSpec);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSpecExists(string id)
        {
            return _context.ProductSpec.Any(e => e.ProSpecID == id);
        }
    }
}
