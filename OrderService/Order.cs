namespace OrderService
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public List<Product> Products { get; set; }
    }
}
