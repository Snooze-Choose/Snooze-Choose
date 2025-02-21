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
            new Product { Name = "Lecker Bierchen", Price=1.30m, LongDescription="Lecker Bierchen ist ein erfrischendes und wohlschmeckendes Bier, das mit ausgewählten Zutaten gebraut wurde.", ShortDescription ="Erfrischendes Bier mit feiner Hopfennote.",Rating = 5.00m, ImageUrl="/images/bier.png"},
            new Product { Name = "Melon", Price=4.30m, LongDescription="Fruchtig, süß und erfrischend – ideal für den Sommer. Gewachsen mit erlesenen Sonnenstrahlen.", ShortDescription ="Erfrischende Melone ohne Kerne.", Rating = 3.9m, ImageUrl="/images/melone.png"},
            new Product { Name = "Mechanische Tastatur", Price = 89.99m, LongDescription = "Ein hochwertiges mechanisches Keyboard mit RGB-Beleuchtung, ideal für Gamer und Vielschreiber.", ShortDescription = "Ergonomisches Design mit präzisem Tastenanschlag.", Rating = 4.85m, ImageUrl = "/images/keyboard.png" },
            new Product { Name = "Kabellose Maus", Price = 29.99m, LongDescription = "Eine kabellose Maus, die ergonomisch geformt ist und sich perfekt für lange Arbeitstage eignet.", ShortDescription = "Präzise und komfortable Maus für den täglichen Gebrauch.", Rating = 4.50m, ImageUrl = "/images/mouse.png" },
            new Product { Name = "Smartphone Hülle", Price = 15.99m, LongDescription = "Hochwertige Handyhülle aus weichem TPU-Material, die dein Smartphone vor Stößen und Kratzern schützt.", ShortDescription = "Schützende Hülle für dein Smartphone in trendigen Farben.", Rating = 4.30m, ImageUrl = "/images/phone_case.png" },
            new Product { Name = "Bluetooth Lautsprecher", Price = 49.99m, LongDescription = "Kompakter Bluetooth-Lautsprecher mit exzellentem Klang und langer Akkulaufzeit – perfekt für unterwegs.", ShortDescription = "Kleiner Lautsprecher mit beeindruckendem Sound.", Rating = 4.75m, ImageUrl = "/images/bluetooth.png" },
            new Product { Name = "Leder Brieftasche", Price = 39.99m, LongDescription = "Ein elegantes und strapazierfähiges Lederportemonnaie mit vielen Fächern für Karten und Geld.", ShortDescription = "Hochwertiges Leder für einen stilvollen Look.", Rating = 3.75m, ImageUrl = "/images/wallet.png" },
            new Product { Name = "Kaffeemaschine", Price = 89.00m, LongDescription = "Vollautomatische Kaffeemaschine für perfekten Kaffeegenuss zu Hause – von Espresso bis Latte.", ShortDescription = "Frisch gebrühter Kaffee, jederzeit. Einfach zu bedienen.", Rating = 4.60m, ImageUrl = "/images/coffeemachine.png" },
            new Product { Name = "Bluetooth Kopfhörer", Price = 59.99m, LongDescription = "Kabellose Over-Ear Kopfhörer mit Noise-Cancelling-Technologie und exzellentem Sound.", ShortDescription = "Genieße Musik ohne störende Umgebungsgeräusche.", Rating = 4.90m, ImageUrl = "/images/headphones.png" },
            new Product { Name = "Smart Watch", Price = 129.99m, LongDescription = "Eine elegante Smartwatch, die Fitness-Tracking, Benachrichtigungen und eine lange Akkulaufzeit bietet.", ShortDescription = "Funktionalität trifft auf Stil – deine Smartwatch für den Alltag.", Rating = 4.70m, ImageUrl = "/images/smartwatch.png" }
        };

            context.AddRange(products);

            context.SaveChanges();
        }
    }
}
