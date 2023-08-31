using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRS_GM.Data;
using PRS_GM.Models;

namespace PRS_GM.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase {
        private readonly AppDbContext _context;

        public RequestsController(AppDbContext context) {
            _context = context;
        }

        private async Task RecalculateOrderTotal(int ID) {
            //this method needs to be implemented after request line is added
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest() {
            if (_context.Request == null) {
                return NotFound();
            }
            return await _context.Request.Include(x=>x.Users).ToListAsync();
        }

        // GET: api/Requests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id) {
            if (_context.Request == null) {
                return NotFound();
            }
            var request = await _context.Request.Include(x => x.Users).Where(x => x.UserID == id).FirstOrDefaultAsync();

            if (request == null) {
                return NotFound();
            }

            return request;
        }

        // PUT: api/Requests/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request) {
            if (id != request.ID) {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) {
                if (!RequestExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Requests
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request) {
            if (_context.Request == null) {
                return Problem("Entity set 'AppDbContext.Request'  is null.");
            }
            _context.Request.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRequest", new { id = request.ID }, request);
        }

        // DELETE: api/Requests/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id) {
            if (_context.Request == null) {
                return NotFound();
            }
            var request = await _context.Request.FindAsync(id);
            if (request == null) {
                return NotFound();
            }

            _context.Request.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id) {
            return (_context.Request?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        //iffy code starts here----------------------------------------------------------------------
        [HttpPut("review/{R}")]
        public async Task<IActionResult> Review(Request R) {
            if (R.Total<=50) {
                R.Status = "APPROVED";
                return await PutRequest(R.ID, R);
            }
            R.Status = "REVIEW"; 
            return await PutRequest(R.ID, R);
        }

        [HttpPut("approve/{R}")]
        public async Task<IActionResult> Approve(Request R) {
            R.Status = "APPROVED";
            return await PutRequest(R.ID,R);
        }

        [HttpPut("reject/{R}")]
        public async Task<IActionResult> Reject(Request R) {
            R.Status = "REJECTED";
            return await PutRequest(R.ID, R);
        }
        //"GetReviews" needs to be rechecked after login method is refined, and requestlines is added
        [HttpGet("reviews/{UID}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetReviews(int UID) {
            if (_context.Request == null) {
                return NotFound();
            }
            var order = await _context.Request.Where(i=>i.Status == "REVIEW").Include(i => i.Users).Where(i => i.UserID != UID).ToListAsync();

            if (order == null) {
                return NotFound();
            }

            return order;
        }
    }
}
