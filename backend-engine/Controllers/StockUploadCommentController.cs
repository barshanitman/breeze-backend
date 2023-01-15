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
    public class StockUploadCommentController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        public StockUploadCommentController(BreezeDataContext context)
        {
            _context = context;
        }

        // GET: api/StockUploadComment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockUploadComment>>> GetStockUploadComments()
        {
          if (_context.StockUploadComments == null)
          {
              return NotFound();
          }
            return await _context.StockUploadComments.ToListAsync();
        }

        // GET: api/StockUploadComment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockUploadComment>> GetStockUploadComment(int id)
        {
          if (_context.StockUploadComments == null)
          {
              return NotFound();
          }
            var stockUploadComment = await _context.StockUploadComments.FindAsync(id);

            if (stockUploadComment == null)
            {
                return NotFound();
            }

            return stockUploadComment;
        }

        [HttpGet("/api/StockUploadComment/StockUpload/{id}")]
        public async Task<ActionResult<StockUploadComment>> GetStockUploadCommentByStockUploadId(int id)
        {
            if (_context.StockUploadComments == null)
            {
                return NotFound();
            }
            var stockUploadComment = await _context.StockUploadComments.OrderByDescending(x => x.UploadedAt).Where(x => x.StockUploadId == id).ToListAsync();

            if (stockUploadComment == null)
            {
                return NotFound();
            }

            return Ok(stockUploadComment);
        }
        

        // PUT: api/StockUploadComment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockUploadComment(int id, StockUploadComment stockUploadComment)
        {
            if (id != stockUploadComment.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockUploadComment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockUploadCommentExists(id))
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
        public async Task<ActionResult<StockUploadComment>> PostStockUploadComment(StockUploadComment stockUploadComment)
        {
          if (_context.StockUploadComments == null)
          {
              return Problem("Entity set 'BreezeDataContext.StockUploadComments'  is null.");
          }
            _context.StockUploadComments.Add(stockUploadComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockUploadComment", new { id = stockUploadComment.Id }, stockUploadComment);
        }

        // DELETE: api/StockUploadComment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockUploadComment(int id)
        {
            if (_context.StockUploadComments == null)
            {
                return NotFound();
            }
            var stockUploadComment = await _context.StockUploadComments.FindAsync(id);
            if (stockUploadComment == null)
            {
                return NotFound();
            }

            _context.StockUploadComments.Remove(stockUploadComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockUploadCommentExists(int id)
        {
            return (_context.StockUploadComments?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
