﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRS_GM.Data;
using PRS_GM.Models;

namespace PRS_GM.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestLinesController : ControllerBase {
        private readonly AppDbContext _context;

        public RequestLinesController(AppDbContext context) {
            _context = context;
        }

        // GET: api/RequestLines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RequestLine>>> GetRequestLine() {
            if (_context.RequestLines == null) {
                return NotFound();
            }
            return await _context.RequestLines.Include(x => x.Product)
                                              .ThenInclude(x => x.Vendor)
                                              .ToListAsync();
        }

        // GET: api/RequestLines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RequestLine>> GetRequestLine(int id) {
            if (_context.RequestLines == null) {
                return NotFound();
            }
            var requestLine = await _context.RequestLines.Include(x => x.Product)
                                                         .ThenInclude(x => x.Vendor)
                                                         .Where(x => x.ID == id)
                                                         .FirstOrDefaultAsync();

            if (requestLine == null) {
                return NotFound();
            }

            return requestLine;
        }

        // PUT: api/RequestLines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequestLine(int id, RequestLine requestLine) {
            if (id != requestLine.ID) {
                return BadRequest();
            }

            _context.Entry(requestLine).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                await RecalcReqTotal(requestLine.RequestID);
            }
            catch (DbUpdateConcurrencyException) {
                if (!RequestLineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RequestLines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RequestLine>> PostRequestLine(RequestLine requestLine) {
            if (_context.RequestLines == null) {
                return Problem("Entity set 'AppDbContext.RequestLine'  is null.");
            }
            _context.RequestLines.Add(requestLine);
            await _context.SaveChangesAsync();
            await RecalcReqTotal(requestLine.RequestID);

            return CreatedAtAction("GetRequestLine", new { id = requestLine.ID }, requestLine);
        }

        // DELETE: api/RequestLines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequestLine(int id) {
            if (_context.RequestLines == null) {
                return NotFound();
            }
            var requestLine = await _context.RequestLines.FindAsync(id);
            if (requestLine == null) {
                return NotFound();
            }

            var IID = requestLine.RequestID;
            _context.RequestLines.Remove(requestLine);
            await _context.SaveChangesAsync();
            await RecalcReqTotal(IID);

            return NoContent();
        }

        private bool RequestLineExists(int id) {
            return (_context.RequestLines?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        private async Task RecalcReqTotal(int id) {
            var total = (from r in _context.Requests
                         join rl in _context.RequestLines on r.ID equals rl.RequestID
                         join p in _context.Products on rl.ProductID equals p.ID
                         where r.ID == id
                         select new { Total = rl.Quantity * p.Price }).Sum(x => x.Total);
            var order = await _context.Requests.FindAsync(id);
            order!.Total = total;
            await _context.SaveChangesAsync();
        }
    }
}
