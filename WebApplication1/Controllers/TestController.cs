using Microsoft.AspNetCore.Mvc;

namespace work_Session.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            var s = HttpContext.Session.GetString("r56");
            ViewBag.msg = s;
            return View();
        }
    }
}
