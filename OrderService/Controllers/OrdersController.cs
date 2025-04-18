using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace OrderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderContext _context;

        public OrdersController(OrderContext context)
        {
            this._context = context;
        }

        [Authorize]
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return _context.Orders
                .Where(o => o.User_id == userId)
                .Include(o => o.Products).ToList();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public ActionResult<Order> Get(int id)
        {
            var order = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // POST api/<OrderController>
        [HttpPost]
        public ActionResult<Order> Post([FromBody] Order order)
        {
            if (order == null) return BadRequest("Order cannot be null");

            var newProducts = new List<Product>();

            foreach (var product in order.Products)
            {
                var existingProduct = _context.Set<Product>().FirstOrDefault(p => p.Id == product.Id);

                if (existingProduct != null)
                {
                    newProducts.Add(existingProduct);
                }
                else
                {
                    newProducts.Add(product);
                }
            }

            order.Products = newProducts;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        [Authorize]
        [HttpPost("logged")]
        public ActionResult<Order> LoggedPost([FromBody] Order order)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (order == null) return BadRequest("Order cannot be null");

            var newProducts = new List<Product>();

            foreach (var product in order.Products)
            {
                var existingProduct = _context.Set<Product>().FirstOrDefault(p => p.Id == product.Id);

                if (existingProduct != null)
                {
                    newProducts.Add(existingProduct);
                }
                else
                {
                    newProducts.Add(product);
                }
            }

            order.Products = newProducts;
            order.User_id = userId;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Order updatedOrder)
        {
            if (updatedOrder == null || id != updatedOrder.Id)
                return BadRequest("Invalid order data");

            var existingOrder = _context.Orders.Include(o => o.Products).FirstOrDefault(o => o.Id == id);

            if (existingOrder == null)
                return NotFound();

            existingOrder.FirstName = updatedOrder.FirstName;
            existingOrder.LastName = updatedOrder.LastName;
            existingOrder.Street = updatedOrder.Street;
            existingOrder.HouseNumber = updatedOrder.HouseNumber;
            existingOrder.City = updatedOrder.City;
            existingOrder.PostalCode = updatedOrder.PostalCode;
            existingOrder.TotalPrice = updatedOrder.TotalPrice;
            existingOrder.PaymentMethod = updatedOrder.PaymentMethod;
            existingOrder.OrderStatus = updatedOrder.OrderStatus;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var order = _context.Orders.Find(id);

            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
