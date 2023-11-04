﻿using Microsoft.AspNetCore.Mvc;
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

        public IActionResult AddNewCategory(int? id)
        {
            ViewBag.category = new SelectList(_context.Category, "CategoryId", "CategoryName", id.ToString() ?? "");
            return PartialView("_addNewCategory");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM, int[] categoryId)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Name = productVM.Name,
                    Unit = productVM.Unit,
                    Price = productVM.Price,
                    Quantity = productVM.Quantity,
                    Description = productVM.Description,
                };

                //Image
                var file = productVM.ImagePath;
                string webroot = _he.WebRootPath;
                string folder = "Images";
                string imgFileName = Path.GetFileName(productVM.ImagePath.FileName);
                string fileToSave = Path.Combine(webroot, folder, imgFileName);


                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        productVM.ImagePath.CopyTo(stream);
                        product.Image = "/" + folder + "/" + imgFileName;

                    }
                }

                foreach (var item in categoryId)
                {
                    ProductCategory productCategory = new ProductCategory()
                    {
                        Product = product,
                        Id = product.Id,
                        CategoryId = item
                    };
                    _context.ProductCategories.Add(productCategory);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
