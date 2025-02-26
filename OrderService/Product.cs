namespace OrderService
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get ; private set; }
        public string? ImageUrl { get; set; }

        public Product(string name, int quantity, decimal unitPrice, string? imageUrl = null)
        {
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
            TotalPrice = unitPrice*quantity;
            ImageUrl = imageUrl;
        }
    }
}
