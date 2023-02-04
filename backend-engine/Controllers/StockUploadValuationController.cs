using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_engine.Models;

namespace backend_engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockUploadValuationController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        public StockUploadValuationController(BreezeDataContext context)
        {
            _context = context;
        }

        // GET: api/StockUploadComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockUploadValuation>>> GetStockUploadValuations()
        {
            if (_context.StockUploadComments == null)
            {
                return NotFound();
            }
            return await _context.StockUploadValuations.ToListAsync();
        }

        // GET: api/StockUploadComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockUploadValuation>> GetStockUploadValuation(int id)
        {
            if (_context.StockUploadValuations == null)
            {
                return NotFound();
            }
            var stockUploadValuation = await _context.StockUploadValuations.FindAsync(id);

            if (stockUploadValuation == null)
            {
                return NotFound();
            }

            return stockUploadValuation;
        }

        [HttpGet("/api/StockUploadValuation/StockUpload/{id}")]
        public async Task<ActionResult<StockUploadValuation>> GetStockUploadValuationByStockUploadId(int id)
        {
            if (_context.StockUploadValuations == null)
            {
                return NotFound();
            }
            var stockUploadValuations = await _context.StockUploadValuations.Where(x => x.StockUploadId == id).ToListAsync();

            if (stockUploadValuations == null)
            {
                return NotFound();
            }

            return Ok(stockUploadValuations);
        }


        // PUT: api/StockUploadComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockUploadValuation(int id, StockUploadValuation stockUploadValuation)
        {
            if (id != stockUploadValuation.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockUploadValuation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockUploadValuationExists(id))
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

        // POST: api/StockUploadComment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StockUploadValuation>> PostStockUploadValuation(StockUploadValuation stockUploadValuations)
        {
            if (_context.StockUploadValuations == null)
            {
                return Problem("Entity set 'BreezeDataContext.StockUploadComments'  is null.");
            }
            _context.StockUploadValuations.Add(stockUploadValuations);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostStockUploadValuation", new { id = stockUploadValuations.Id }, stockUploadValuations);
        }

        // DELETE: api/StockUploadComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockUploadComment(int id)
        {
            if (_context.StockUploadValuations == null)
            {
                return NotFound();
            }
            var stockUploadValuations = await _context.StockUploadValuations.FindAsync(id);
            if (stockUploadValuations == null)
            {
                return NotFound();
            }

            _context.StockUploadValuations.Remove(stockUploadValuations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockUploadValuationExists(int id)
        {
            return (_context.StockUploadValuations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
