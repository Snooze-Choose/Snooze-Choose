using Microsoft.EntityFrameworkCore;

namespace OrderService
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options) { }

        public DbSet<Order> Orders { get; set; } = default!;
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public static class Extensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<OrderContext>();
            try
            {
                context.Database.EnsureCreated();
                DbInitializer.Initialize(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Datenbankinitialisierung fehlgeschlagen: {ex.Message}");
            }
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(OrderContext context)
        {
            if (context.Orders.Any())
            {
                return;
            }

            var products = new List<Product>
            {
                new Product("Lecker Bierchen", 12, 0.76m),
            };

            var orders = new List<Order>
            {
                new Order
                {
                    FirstName = "Hans",
                    LastName = "Müller",
                    Street = "Teststraße",
                    HouseNumber = "4",
                    City = "Berlin",
                    PostalCode = "10115",
                    Products = products,
                    TotalPrice = 9.12m,
                    PaymentMethod = "Kreditkarte",
                    CardHolderName = "Hans Müller",
                    CardNumber = "1234567812345678",
                    ExpiryDate = "12/25",
                    CVV = "123",
                    OrderDate = DateTime.UtcNow,
                    OrderStatus = "Ausstehend"
                }
            };

            context.AddRange(orders);
            context.SaveChanges();
        }
    }
}
