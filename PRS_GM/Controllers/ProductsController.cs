using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRS_GM.Data;
using PRS_GM.Models;

namespace PRS_GM.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context) {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct() {
            if (_context.Products == null) {
                return NotFound();
            }
            return await _context.Products.Include(x => x.Vendor)
                                          .ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {
            if (_context.Products == null) {
                return NotFound();
            }
            var product = await _context.Products.Include(x => x.Vendor)
                                                 .Where(x => x.ID == id)
                                                 .FirstOrDefaultAsync();
            
            if (product == null) {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product) {
            if (id != product.ID) {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!ProductExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product) {
            if (_context.Products == null) {
                return Problem("Entity set 'AppDbContext.Product'  is null.");
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.ID }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id) {
            if (_context.Products == null) {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null) {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id) {
            return (_context.Products?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
