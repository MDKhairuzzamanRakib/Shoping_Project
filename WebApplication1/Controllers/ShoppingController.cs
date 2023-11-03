using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class ShoppingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
