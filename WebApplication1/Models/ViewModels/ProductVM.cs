namespace WebApplication1.Models.ViewModels
{
    public class ProductVM
    {
        public ProductVM()
        {
            this.CategoryList = new List<int>();
            this.SpecificationList = new List<int>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Unit { get; set; } = default!;
        public double Price { get; set; }
        public double Quantity { get; set; }
        public string? Image { get; set; } = default!;
        public IFormFile? ImagePath { get; set; }
        public string Description { get; set; } = default!;

        public List<int> CategoryList { get; set; }
        public List<int> SpecificationList { get; set; }
    }
}
