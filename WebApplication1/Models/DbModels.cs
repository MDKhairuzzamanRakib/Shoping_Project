using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; } = default!;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = default!;
        public int Role { get; set; }
        public int IsActive { get; set; }
    }

    public class Category
    {
        public Category()
        {
            this.ProductCategories = new List<ProductCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;

        //nev
        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
    }

    public class Product
    {
        public Product()
        {
            this.ProductCategories = new List<ProductCategory>();
            this.SalesItems = new List<SalesItem>();
        }
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Unit { get; set; } = default!;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string? Image { get; set;} = default!;
        public string Description { get; set; } = default!;


        public virtual ICollection<ProductCategory>? ProductCategories { get; set; }
        public virtual ICollection<SalesItem>? SalesItems { get; }
    }

    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        [ForeignKey("Product")]
        public int Id { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Product? Product { get; set; }

    }


    public class Customer
    {
        public Customer()
        {
            this.SalesOrders = new List<SalesOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Mobile { get; set; } = default!;
        public string Address { get; set; } = default!;

        //Nev
        public virtual ICollection<SalesOrder>? SalesOrders { get; set; }
    }

    public class SalesOrder
    {
        public SalesOrder()
        {
            this.SalesItems = new List<SalesItem>();
        }

        public int Id { get; set; }
        public string OrderNo { get; set; } = default!;
        public DateTime OrderDate { get; set; }

        //nev
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        public virtual ICollection<SalesItem>? SalesItems { get;}
    }
    public class SalesItem
    {
        public int Id { get; set; }

        //nev
        [ForeignKey("SalesOrder")]
        public int SalesOrderId { get; set; }

        public virtual SalesOrder? SalesOrder { get; set; }

        public double UnitPrice { get; set; }
        public double Quantity { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product? Product { get; set; }
    }


    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions<ShopDbContext> options):base(options)
        {
            
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> SalesOrders { get; set; }
        public DbSet<SalesItem> SalesItems { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Category> Category { get; set; }

    }

}
