﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models.ViewModels;
using WebApplication1.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace WebApplication1.Controllers
{
    public class AddNewProductsController : Controller
    {
        private readonly ShopDbContext _context;
        private readonly IWebHostEnvironment _he;
        public AddNewProductsController(ShopDbContext context, IWebHostEnvironment he)
        {
            this._context = context;
            this._he = he;
        }
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.msg = TempData["msg"];
            return View(await _context.Products.Include(x => x.ProductCategories).ThenInclude(y => y.Category).ToListAsync());
        }

        public IActionResult AddNewCategory(int? id)
        {
            ViewBag.category = new SelectList(_context.Category, "CategoryId", "CategoryName", id.ToString() ?? "");
            return PartialView("_addNewCategory");
        }

        public IActionResult AddNewSpecification(int? id)
        {
            ViewBag.specName = new InputText();
            ViewBag.specDetails = new InputText();
            return PartialView("_addNewSpecification");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM productVM, int categoryId)
        {
            string msg = "";
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
                Specification specification = new Specification()
                {
                    SpecificationName = productVM.SpecificationName,
                    SpecificationDetails = productVM.SpecificationDetails,
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

                
                ProductCategorySpecification productCategory = new ProductCategorySpecification()
                {
                    Product = product,
                    ProductId = product.Id,
                    Specification = specification,
                    SpecificationId = specification.SpecificationId,
                    CategoryId = categoryId
                };
                _context.ProductCategories.Add(productCategory);
                


                await _context.SaveChangesAsync();
                msg = "Product Created Sucssefully!!";
                return RedirectToAction(nameof(AddProduct));

            }
            TempData["msg"] = msg;
            return RedirectToAction(nameof(AddProduct));
        }

        public IActionResult Edit(int? id)
        {
            Product product = _context.Products.First(x => x.Id == id);
            var productCategory = _context.ProductCategories.Where(x=>x.ProductId == id).ToList();
            
            

            ProductVM productVM = new ProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Unit = product.Unit,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                Description = product.Description,
            };
            

            foreach (var item in productCategory)
            {
                productVM.CategoryList.Add(item.CategoryId);
                productVM.SpecificationList.Add(item.SpecificationId);
            }
            return View(productVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM productVM, int categoryId,int specificationId)
        {
            if (ModelState.IsValid)
            {
                Product product = new Product()
                {
                    Id = productVM.Id,
                    Name = productVM.Name,
                    Unit = productVM.Unit,
                    Price = productVM.Price,
                    Quantity = productVM.Quantity,
                    Description = productVM.Description
                };
                Specification specification = new Specification()
                {
                    SpecificationName = productVM.SpecificationName,
                    SpecificationDetails = productVM.SpecificationDetails,
                };
                var file = productVM.ImagePath;
                if (file!=null)
                {
                    string webroot = _he.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Path.GetFileName(productVM.ImagePath.FileName);
                    string fileToSave = Path.Combine(webroot, folder, imgFileName);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        productVM.ImagePath.CopyTo(stream);
                        product.Image = "/" + folder + "/" + imgFileName;
                    }
                }

                var existItem = _context.ProductCategories.Where(x => x.ProductId == product.Id).ToList();
                foreach (var item in existItem)
                {
                    _context.ProductCategories.Remove(item);

                }

                
                ProductCategorySpecification productCategory = new ProductCategorySpecification()
                {
                    ProductId = product.Id,
                    CategoryId = categoryId,
                    SpecificationId = product.Id
                };
                _context.ProductCategories.Add(productCategory);

                _context.UpdateRange(product,specification);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AddProduct));
            }
            return View();
        }
         public IActionResult Delete(int? id)
        {
            Product product = _context.Products.First(x => x.Id == id);
            var productCategory = _context.ProductCategories.Where(x => x.ProductId == id).ToList();

            ProductVM productVM = new ProductVM()
            {
                Id = product.Id,
                Name = product.Name,
                Unit = product.Unit,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description
            };

            foreach (var item in productCategory)
            {
                productVM.CategoryList.Add(item.CategoryId);
            }
            return View(productVM);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Product product = _context.Products.Find(id);
            _context.Entry(product).State = EntityState.Deleted;
            _context.SaveChanges();

            return RedirectToAction(nameof(AddProduct));
        }

    }
}
