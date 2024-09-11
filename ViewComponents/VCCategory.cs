using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class VCCategory : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public VCCategory(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id="00")
        {
            var category = await _context.Category.Include(c => c.ParentCategoryNavigation)
                                                          .Skip(1)
                                                          .Where(c => c.ParentCategory == id)
                                                          .OrderBy(c => c.ParentCategory)
                                                          .ThenBy(c => c.CategoryID)
                                                          .ToListAsync();
            return View(category);
        }
    }
}
