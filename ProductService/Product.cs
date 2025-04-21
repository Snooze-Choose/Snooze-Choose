namespace ProductService
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public decimal Rating { get; set; }
        public string? ImageUrl { get; set; }
        public string category { get; set; }

    }
}
