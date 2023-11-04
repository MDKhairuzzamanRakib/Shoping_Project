using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ShoppingController : Controller
    {
        private ShopDbContext db;
        public ShoppingController(ShopDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            ViewBag.msg = TempData["msg"];
            return View(db.Products.ToList());
        }

        public IActionResult AddToCart(int pid, double qty)
        {

            return View();
        }
    }
}
