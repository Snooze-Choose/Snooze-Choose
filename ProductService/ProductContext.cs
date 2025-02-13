using Microsoft.EntityFrameworkCore;

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
            new Product { Name = "Lecker Bierchen", Price=1.30m, LongDescription="Lecker Bierchen ist ein erfrischendes und wohlschmeckendes Bier, das mit ausgewählten Zutaten gebraut wurde.", ShortDescription ="Erfrischendes Bier mit feiner Hopfennote.", ImageUrl="/images/bier.png"},
            new Product { Name = "Melon", Price=4.30m, LongDescription="Fruchtig, süß und erfrischend – ideal für den Sommer. Gewachsen mit erlesenen Sonnenstrahlen.", ShortDescription ="Erfrischende Melone ohne Kerne.", ImageUrl="/images/melone.png"},
        };

            context.AddRange(products);

            context.SaveChanges();
        }
    }
}
