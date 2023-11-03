﻿using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString("r56", "Web Development using C#");
            return View();
        }
    }
} 
