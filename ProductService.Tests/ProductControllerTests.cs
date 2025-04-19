using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Controllers;
using Xunit;
using System.Linq;

namespace ProductService.Tests
{
    public class ProductsControllerTests
    {
        private ProductContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid())
                .Options;

            var context = new ProductContext(options);

            context.Products.AddRange(
                new Product { Name = "Lecker Bierchen", Kategorie = Kategorie.Nahrung, Price = 1.30m, LongDescription = "Lecker Bierchen ist ein erfrischendes und wohlschmeckendes Bier, das mit ausgewählten Zutaten gebraut wurde.", ShortDescription = "Erfrischendes Bier mit feiner Hopfennote.", Rating = 5.00m, ImageUrl = "/images/bier.png" },
                new Product { Name = "Melon", Kategorie = Kategorie.Nahrung, Price = 4.30m, LongDescription = "Fruchtig, süß und erfrischend – ideal für den Sommer. Gewachsen mit erlesenen Sonnenstrahlen.", ShortDescription = "Erfrischende Melone ohne Kerne.", Rating = 3.9m, ImageUrl = "/images/melone.png" }
            );

            context.SaveChanges();

            return context;
        }

        [Fact]
        public void Get_Returns_All_Products()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = controller.Get(null);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Get_With_Kategorie_Filter_Returns_Filtered()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = controller.Get(Kategorie.Nahrung);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Post_Adds_New_Product()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var newProduct = new Product { Name = "Lecker Bierchen", Kategorie = Kategorie.Nahrung, Price = 1.30m, LongDescription = "Lecker Bierchen ist ein erfrischendes und wohlschmeckendes Bier, das mit ausgewählten Zutaten gebraut wurde.", ShortDescription = "Erfrischendes Bier mit feiner Hopfennote.", Rating = 5.00m, ImageUrl = "/images/bier.png" };

            var result = controller.Post(newProduct);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnedProduct = Assert.IsType<Product>(createdResult.Value);

            Assert.Equal("Lecker Bierchen", returnedProduct.Name);
            Assert.Equal(3, context.Products.Count());
        }

        [Fact]
        public void Post_With_Null_Product_Returns_BadRequest()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = controller.Post(null);

            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public void Put_With_Valid_Id_Updates_Product()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var productToUpdate = context.Products.First();
            productToUpdate.Name = "Updated Name";

            var result = controller.Put(productToUpdate.Id, productToUpdate);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Updated Name", context.Products.Find(productToUpdate.Id)!.Name);
        }

        [Fact]
        public void Put_With_Mismatched_Id_Returns_BadRequest()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var productToUpdate = context.Products.First();
            var result = controller.Put(productToUpdate.Id + 1, productToUpdate);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void Delete_Removes_Existing_Product()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Single(context.Products);
        }

        [Fact]
        public void Delete_With_Invalid_Id_Returns_NotFound()
        {
            var context = GetInMemoryDbContext();
            var controller = new ProductsController(context);

            var result = controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
