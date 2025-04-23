using System;
using System.Collections.Generic;

namespace OrderService
{
    public class Order
    {
        public int Id { get; set; }
        public string? User_id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public List<Product> Products { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; } 
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string OrderStatus { get; set; } = "Ausstehend";
    }
}
