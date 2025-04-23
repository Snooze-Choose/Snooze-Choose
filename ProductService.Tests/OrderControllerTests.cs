using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService;
using OrderService.Controllers;
using System.Security.Claims;
using Xunit;

namespace OrderService.Tests
{
    public class OrdersControllerTests
    {
        private OrderContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderContext>()
                .UseInMemoryDatabase("OrderDb_" + Guid.NewGuid())
                .Options;

            var context = new OrderContext(options);

            context.Orders.Add(new Order
            {
                Id = 1,
                FirstName = "Max",
                LastName = "Mustermann",
                Street = "Teststraße",
                HouseNumber = "1A",
                City = "Teststadt",
                PostalCode = "12345",
                User_id = "user-123",
                Products = new List<Product>
                {
                    new Product("Bier", 2, 1.5m)
                },
                TotalPrice = 3.0m,
                PaymentMethod = "PayPal",
                CardHolderName = "Max Mustermann",
                CardNumber = "1234567890123456",
                ExpiryDate = "12/25",
                CVV = "123"
            });

            context.SaveChanges();
            return context;
        }

        private OrdersController GetControllerWithUser(OrderContext context, string userId)
        {
            var controller = new OrdersController(context);
            var claims = new List<Claim> { new Claim(ClaimTypes.NameIdentifier, userId) };
            var identity = new ClaimsIdentity(claims);
            var user = new ClaimsPrincipal(identity);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            return controller;
        }

        [Fact]
        public void Get_Returns_Orders_For_User()
        {
            var context = GetInMemoryDbContext();
            var controller = GetControllerWithUser(context, "user-123");

            var result = controller.Get();

            Assert.Single(result);
        }

        [Fact]
        public void GetForAdmin_Returns_All_Orders()
        {
            var context = GetInMemoryDbContext();
            var controller = GetControllerWithUser(context, "admin");

            var result = controller.GetForAdmin();

            Assert.Single(result);
        }

        [Fact]
        public void GetById_Returns_Correct_Order()
        {
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var result = controller.Get(1);
            var actionResult = Assert.IsType<ActionResult<Order>>(result);
            var order = Assert.IsType<Order>(actionResult.Value);

            Assert.Equal(1, order.Id);
        }

        [Fact]
        public void Post_Creates_New_Order()
        {
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var newOrder = new Order
            {
                FirstName = "Anna",
                LastName = "Schmidt",
                Street = "Straße 2",
                HouseNumber = "3",
                City = "Hamburg",
                PostalCode = "20095",
                PaymentMethod = "Kreditkarte",
                CardHolderName = "Anna Schmidt",
                CardNumber = "9876543210987654",
                ExpiryDate = "11/26",
                CVV = "456",
                Products = new List<Product>
                {
                    new Product("Melone", 1, 4.5m)
                }
            };

            var result = controller.Post(newOrder);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returned = Assert.IsType<Order>(created.Value);

            Assert.Equal(4.5m, returned.TotalPrice);
            Assert.Equal(2, context.Orders.Count());
        }

        [Fact]
        public void LoggedPost_Saves_Order_With_UserId()
        {
            var context = GetInMemoryDbContext();
            var controller = GetControllerWithUser(context, "user-999");

            var newOrder = new Order
            {
                FirstName = "Chris",
                LastName = "Test",
                Street = "Blumenweg",
                HouseNumber = "4",
                City = "Berlin",
                PostalCode = "10115",
                PaymentMethod = "PayPal",
                CardHolderName = "Chris Test",
                CardNumber = "0000111122223333",
                ExpiryDate = "01/27",
                CVV = "789",
                Products = new List<Product>
                {
                    new Product("Pizza", 1, 8.0m)
                }
            };

            var result = controller.LoggedPost(newOrder);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var order = Assert.IsType<Order>(created.Value);

            Assert.Equal("user-999", order.User_id);
        }

        [Fact]
        public void Put_Updates_Order()
        {
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var updated = context.Orders.First();
            updated.City = "Neue Stadt";

            var result = controller.Put(updated.Id, updated);

            Assert.IsType<NoContentResult>(result);
            Assert.Equal("Neue Stadt", context.Orders.Find(updated.Id)!.City);
        }

        [Fact]
        public void Delete_Removes_Order()
        {
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var result = controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(context.Orders);
        }

        [Fact]
        public void Delete_Returns_NotFound_If_Invalid()
        {
            var context = GetInMemoryDbContext();
            var controller = new OrdersController(context);

            var result = controller.Delete(999);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
