using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            this._context = context; 
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public IEnumerable<Product> Get([FromQuery] Kategorie? kategorie)
        {
            if (kategorie.HasValue)
            {
                return _context.Products.Where(p => p.Kategorie == kategorie.Value).ToList();
            }

            return _context.Products.ToList();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductsController>
        [Authorize]
        [HttpPost]
        public ActionResult<Product> Post([FromBody] Product product)
        {
            if(product == null) return BadRequest("Product cannot be null");

            _context.Products.Add(product);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }

        // PUT api/<ProductsController>/5
        [Authorize]
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Product product)
        {
            if (id != product.Id) return BadRequest("Product ID mismatch");
            
            _context.Entry(product).State = EntityState.Modified;

            try
            {
               _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/<ProductsController>/5
        [Authorize]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null) return NotFound();

            _context.Products.Remove(product);

            _context.SaveChanges();

            return NoContent();
        }

        [Authorize]
        [HttpPost("{id}/upload")]
        public async Task<IActionResult> UploadImage(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid file");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound("Product not found");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            product.ImageUrl = $"/images/{uniqueFileName}";
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return Ok(new { ImageUrl = product.ImageUrl });
        }

    }
}
