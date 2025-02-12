using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<OrderController>
     
        [HttpGet]
        public IEnumerable<Order> Get()
        {
            return _context.Orders.Include(o => o.Products).ToList();
        }

        // GET api/<OrderController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<OrderController>
        [HttpPost]
        public ActionResult<Order> Post([FromBody] Order order)
        {
            if (order == null) return BadRequest("Order cannot be null");


            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OrderController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
