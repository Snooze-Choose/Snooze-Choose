namespace ProductService.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public int Stars { get; set; }
        public required Category Category { get; set; }
    }
}
