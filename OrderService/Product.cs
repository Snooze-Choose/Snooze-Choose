namespace OrderService
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get ; private set; }

        public Product(string name, int amount, decimal unitPrice)
        {
            Name = name;
            Amount = amount;
            UnitPrice = unitPrice;
            TotalPrice = unitPrice*amount;
        }
    }
}
