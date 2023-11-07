using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebApplication1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IWebHostEnvironment _he;
        public ProductsController(ShopDbContext context, IWebHostEnvironment he)
        {
            this._context = context;
            this._he = he;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.Include(x => x.ProductCategories).ThenInclude(y => y.Category).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            Product product = _context.Products.First(x => x.Id == id);
            var productCategory = _context.ProductCategories.Where(x => x.ProductId == id).ToList();

            return View(product);
        }
        
    }
}
