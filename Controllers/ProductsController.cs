using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;
using CottonDollStore.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;
using System.IO;
using DbCottonDollStore.ViewComponents;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DbCottonDollStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public ProductsController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var cottonDollStoreContext = _context.Product.Include(p => p.Category).Include(p => p.CategoryID_2Navigation).Include(p => p.CategoryID_3Navigation).Include(p => p.Store);
            return View(await cottonDollStoreContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            VMProductSpec product = new VMProductSpec()
            {
                Products = await _context.Product.Where(p => p.ProID == id).ToListAsync(),
                Store = await _context.Store.Where(s => _context.Product.Any(p => p.StoreID == s.StoreID && p.ProID == id)).ToListAsync(),
                Category = await _context.Category.Where(c => _context.Product.Any(p => p.ProID == id && p.CategoryID == c.CategoryID)).ToListAsync(),
                Spec = await _context.ProductSpec.Where(p => p.ProID == id).ToListAsync()
            };

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        
        // GET: Products/Create
        public IActionResult Create(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["sid"] = id;
            return RedirectToAction("CreateVM", new { id });
        }

        public IActionResult CreateVM(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["sid"] = id;

            ViewData["CategoryID"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory != "01" &&  c.CategoryID != "01" && c.ParentCategory != "05" && c.CategoryID != "05"), "CategoryID", "CategoryName");
            ViewData["CategoryID_2"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "01" || c.CategoryID == "01"), "CategoryID", "CategoryName");
            ViewData["CategoryID_3"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "05"), "CategoryID", "CategoryName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(/*IFormFile? photo,*/ IFormFile[]? photos, VMProduct product)
        {
            var pid = _context.Product
                            .FromSqlRaw("SELECT dbo.getProID() AS ProID")
                            .Select(p => p.ProID)
                            .AsEnumerable()
                            .FirstOrDefault();

            var newproduct = new Product
            {
                ProID = pid,
                ProName = product.ProName,
                Information = product.Information,
                CategoryID = product.CategoryID,
                CategoryID_2 = product.CategoryID_2,
                CategoryID_3 = product.CategoryID_3,
                UpdateTime = DateTime.Now,
                Clicks = 0,
                Statu = product.Spec.All(ps => ps.Quantity == 0),
                ProImg = product.ProImg,
                StoreID = product.StoreID,
                ProductSpec = product.Spec

            };
            


            //if (photo != null && photo.Length > 0)
            //{
            //    if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
            //    {
            //        ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
            //        return View();
            //    }
            //    //string fileName = Path.GetFileName(photo.FileName);
            //    // 确定上传目录的路径為 wwwroot/Photos ，並存成uploadPath
            //    newproduct.ProImg = newproduct.ProID + Path.GetExtension(photo.FileName);
            //    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "prods", newproduct.ProImg);

            //    // 使用文件流將文件上傳至伺服器
            //    using (var stream = new FileStream(uploadPath, FileMode.Create))
            //    {
            //        photo.CopyTo(stream);
            //    }
            //    ViewData["Message"] = "上傳成功!";

            //} else {
            //    ViewData["Message"] = "沒有檔案，請檢察正確性";
            //}

            if (photos.Length == 0)
            {

                ViewData["Message"] = "沒有檔案，請檢察正確性!!";
                return View();
            }

            foreach (IFormFile SpecPhoto in photos)
            {
                if (SpecPhoto.ContentType != "image/jpeg" && SpecPhoto.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View();
                }
            }

            int i = 0;
            string[] FN = new string[photos.Length];

            foreach (IFormFile SpecPhoto in photos)
            {
                
                var newSpecImg = newproduct.ProID +"_"+ i + Path.GetExtension(SpecPhoto.FileName);
                FN[i] = Path.GetExtension(SpecPhoto.FileName);
                if (i == 0)
                {
                    newproduct.ProImg = newSpecImg;
                } 

                string uploadPaths = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "prod", newSpecImg);

                // 把檔案上傳至伺服器儲存
                using (var streams = new FileStream(uploadPaths, FileMode.Create))
                {
                    SpecPhoto.CopyTo(streams);
                }

                i++;
            }

            i = 1;
            foreach ( var spec in newproduct.ProductSpec)
            {
                if (i < FN.Length)
                {
                    spec.SpecImg = newproduct.ProID + "_" + i + FN[i];
                }
                i++;
            }

            if (ModelState.IsValid)
            {
                _context.Add(newproduct);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Stores", new { id = product.StoreID });
            }

            ViewData["CategoryID"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory != "01" && c.CategoryID != "01" && c.ParentCategory != "05" && c.CategoryID != "05"), "CategoryID", "CategoryName", product.CategoryID);
            ViewData["CategoryID_2"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "01" || c.CategoryID == "01"), "CategoryID", "CategoryName", product.CategoryID_2);
            ViewData["CategoryID_3"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "05"), "CategoryID", "CategoryName", product.CategoryID_3);
            ViewData["proid"] = product.ProID;
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p => p.ProductSpec).Where(p => p.ProID == id).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            var newproduct = new VMProduct
            {
                ProID = product.ProID,
                ProName = product.ProName,
                Information = product.Information,
                CategoryID = product.CategoryID,
                CategoryID_2 = product.CategoryID_2,
                CategoryID_3 = product.CategoryID_3,
                UpdateTime = product.UpdateTime,
                Clicks = product.Clicks,
                Statu = product.Statu,
                ProImg = product.ProImg,
                StoreID = product.StoreID,
                Spec = product.ProductSpec.ToList()
            };

            ViewData["CategoryID"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory != "01" && c.CategoryID != "01" && c.ParentCategory != "05" && c.CategoryID != "05"), "CategoryID", "CategoryName", product.CategoryID);
            ViewData["CategoryID_2"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "01" || c.CategoryID == "01"), "CategoryID", "CategoryName", product.CategoryID_2);
            ViewData["CategoryID_3"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "05"), "CategoryID", "CategoryName", product.CategoryID_3);
            return View(newproduct);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile[]? photos, VMProduct product)
        {
            var result = await _context.Product.Where(p => p.ProID == product.ProID).Include(p => p.ProductSpec).FirstOrDefaultAsync();

            if (result == null)
            {
                return View(product);
            }

            result.ProName = product.ProName;
            result.Information = product.Information;
            result.Statu = product.Spec.All(ps => ps.Quantity == 0);
            result.CategoryID = product.CategoryID;
            result.CategoryID_2 = product.CategoryID_2;
            result.CategoryID_3 = product.CategoryID_3;
            //result.ProductSpec = product.Spec;


            foreach (IFormFile SpecPhoto in photos)
            {
                if (SpecPhoto.ContentType != "image/jpeg" && SpecPhoto.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View();
                }
            }

            int i = 0;
            string[] FN = new string[photos.Length];

            foreach (IFormFile SpecPhoto in photos)
            {

                var newSpecImg = result.ProID + "_" + i + Path.GetExtension(SpecPhoto.FileName);
                FN[i] = Path.GetExtension(SpecPhoto.FileName);
                if (i == 0)
                {
                    result.ProImg = newSpecImg;
                }

                string uploadPaths = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "prod", newSpecImg);

                // 把檔案上傳至伺服器儲存
                using (var streams = new FileStream(uploadPaths, FileMode.Create))
                {
                    SpecPhoto.CopyTo(streams);
                }

                i++;
            }

            i = 1;
            foreach (var spec in product.Spec)
            {
                if (i < FN.Length)
                {
                    spec.SpecImg = result.ProID + "_" + i + FN[i];
                }
                i++;
            }
        

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ProductSpec.RemoveRange(result.ProductSpec);
                    result.ProductSpec = product.Spec;
                    _context.Update(result);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Stores", new { id = product.StoreID });
            }
            ViewData["CategoryID"] = new SelectList(_context.Category.Where(c => c.CategoryID != "00" && c.ParentCategory != "01" && c.CategoryID != "01" && c.ParentCategory != "05" && c.CategoryID != "05"), "CategoryID", "CategoryName", product.CategoryID);
            ViewData["CategoryID_2"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "01" || c.CategoryID == "01"), "CategoryID", "CategoryName", product.CategoryID_2);
            ViewData["CategoryID_3"] = new SelectList(_context.Category.Where(c => c.ParentCategory == "05"), "CategoryID", "CategoryName", product.CategoryID_3);
            return Json(product);
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Removed(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var product = await _context.Product.FindAsync(id);

        //    if (product != null)
        //    {
        //        product.Statu = true;
        //        _context.Product.Update(product);
        //    }

        //    await _context.SaveChangesAsync();
        //    return Json(product);
        //}


        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .Include(p => p.CategoryID_2Navigation)
                .Include(p => p.CategoryID_3Navigation)
                .Include(p => p.Store)
                .FirstOrDefaultAsync(m => m.ProID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var product = await _context.Product.Include(p => p.ProductSpec).Where(p => p.ProID == id).FirstOrDefaultAsync();
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(string id)
        {
            return _context.Product.Any(e => e.ProID == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Clicks(string ProID)
        {
            if (ProID == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(ProID);

            if (product != null)
            {
                product.Clicks += 1;
                _context.Product.Update(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", new { id = ProID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SellDown(string ProID)
        {
            if (ProID == null)
            {
                return NotFound();
            }

            var product = await _context.Product.Include(p => p.ProductSpec).Where(p => p.ProID == ProID).FirstOrDefaultAsync();

            if (product != null)
            {
                //if (product.ProductSpec.All(ps => ps.Quantity == 0))
                //{
                //    product.Statu = true;
                //    _context.Product.Update(product);
                //    await _context.SaveChangesAsync();
                //    ViewData["erroeMessage"] = ""
                //    return RedirectToAction("Details", "Stores", new { id = product.StoreID });

                //}
                    product.Statu = product.Statu == true? false: true;
                _context.Product.Update(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Stores", new { id = product.StoreID });
        }

       
    }
}
