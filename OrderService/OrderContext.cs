using Microsoft.EntityFrameworkCore;

namespace OrderService
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {

        }

        public DbSet<Order> Orders { get; set; } = default!;
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
            }
        }
    }
    public static class DbInitializer
    {
        public static void Initialize(OrderContext context)
        {
            if (context.Orders.Any())
                return;

            var products = new List<Product>
            { 
                new Product("Lecker Bierchen", 12, 0.76m),
                new Product("Drogen", 100, 10.0m)
            };

            var orders = new List<Order>
            {
                new Order { CustomerName="Marvin", Address="Vollsteadt", Products=products },
            };

            context.AddRange(orders);

            context.SaveChanges();
        }
    }
}
