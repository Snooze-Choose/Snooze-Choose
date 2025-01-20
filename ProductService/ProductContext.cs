using Microsoft.EntityFrameworkCore;
using ProductService.Models;

namespace ProductService
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options){}

        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
    }
    public static class Extensions
    {
        public static void CreateDbIfNotExists(this IHost host)
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<ProductContext>();
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
        public static void Initialize(ProductContext context)
        {
            if (context.Products.Any() || context.Categories.Any())
                return;

            var outdoor = new Category() { Name = "Outdoor" };

            var products = new List<Product>
        {
            new Product { Name = "Solar Powered Flashlight", Price = 5.0m, Stars=4, Description="Eine solar betriebene Lampe", Category=outdoor },
        };

            context.Add(outdoor);
            context.AddRange(products);

            context.SaveChanges();
        }
    }
}
