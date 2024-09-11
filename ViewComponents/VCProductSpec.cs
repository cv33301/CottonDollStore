using CottonDollStore.ViewModels;
using DbCottonDollStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbCottonDollStore.ViewComponents
{
    public class VCProductSpec : ViewComponent
    {
        private readonly CottonDollStoreContext _context;

        public VCProductSpec(CottonDollStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id, string sid)
        {

            ViewData["pid"] = id;
            ViewData["sid"] = sid;

            return View();
        }
    }
}
