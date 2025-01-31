﻿using Microsoft.EntityFrameworkCore;

namespace ProductService
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; } = default!;
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
            if (context.Products.Any())
                return;

            var products = new List<Product>
        {
            new Product { Name = "Solar Powered Flashlight"},
        };

            context.AddRange(products);

            context.SaveChanges();
        }
    }
}
