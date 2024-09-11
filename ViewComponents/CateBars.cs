using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class CateBars : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public CateBars(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var category = _context.Category.Include(c => c.ProductCategoryID_2Navigation)
                                            .Include(c => c.ProductCategoryID_3Navigation);

            return View(await category.ToListAsync());
        }
    }
}
