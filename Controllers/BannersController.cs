using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DbCottonDollStore.Models;

namespace DbCottonDollStore.Controllers
{
    public class BannersController : Controller
    {
        private readonly CottonDollStoreContext _context;

        public BannersController(CottonDollStoreContext context)
        {
            _context = context;
        }

        // GET: Banners
        public async Task<IActionResult> Index()
        {
            return View(await _context.Banner.ToListAsync());
        }

        // GET: Banners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.BID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // GET: Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Banners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BID,BannerImg,Information,StartDate,EndDate")] Banner banner, IFormFile photo)
        {

            if (photo != null && photo.Length > 0)
            {
                if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View(banner);
                }
                //string fileName = Path.GetFileName(photo.FileName);
                // 确定上传目录的路径為 wwwroot/Photos ，並存成uploadPath
                banner.BannerImg = banner.BID + Path.GetExtension(photo.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "banner", banner.BannerImg);

                // 使用文件流將文件上傳至伺服器
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                ViewData["Message"] = "上傳成功!";

            }

            if (ModelState.IsValid)
            {
                _context.Add(banner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(banner);
        }

        // GET: Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner.FindAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            return View(banner);
        }

        // POST: Banners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Banner banner, IFormFile? photo)
        {
            if (banner == null)
            {
                return NotFound();
            }


            if (photo != null && photo.Length > 0)
            {
                if (photo.ContentType != "image/jpeg" && photo.ContentType != "image/png")
                {
                    ViewData["Message"] = "只允許上傳 JPEG 或 PNG 格式的圖片文件。";
                    return View(banner);
                }
                //string fileName = Path.GetFileName(photo.FileName);
                // 确定上传目录的路径為 wwwroot/Photos ，並存成uploadPath
                banner.BannerImg = banner.BID + Path.GetExtension(photo.FileName);
                string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "banner", banner.BannerImg);

                // 使用文件流將文件上傳至伺服器
                using (var stream = new FileStream(uploadPath, FileMode.Create))
                {
                    photo.CopyTo(stream);
                }
                ViewData["Message"] = "上傳成功!";

            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(banner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(banner.BID))
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
            return View(banner);
        }

        // GET: Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var banner = await _context.Banner
                .FirstOrDefaultAsync(m => m.BID == id);
            if (banner == null)
            {
                return NotFound();
            }

            return View(banner);
        }

        // POST: Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var banner = await _context.Banner.FindAsync(id);
            if (banner != null)
            {
                _context.Banner.Remove(banner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banner.Any(e => e.BID == id);
        }
    }
}
