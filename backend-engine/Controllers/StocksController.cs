using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_engine.Models;
using backend_engine.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace backend_engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class StocksController : ControllerBase
    {
        private readonly BreezeDataContext _context;
        private readonly IRepository<Stock> _repo;

        public StocksController(BreezeDataContext context, IRepository<Stock> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/Stocks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            if (_context.Stocks == null)
            {
                return NotFound();
            }
            return Ok(await _repo.GetAll());
        }

        // GET: api/Stocks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetStock(int id)
        {
            if (_context.Stocks == null)
            {
                return NotFound();
            }
            //Stock stock = await _repo.GetById(id);
            object stock = _context.Stocks.Where(x => x.Id == id).Include(x => x.StockUploads.OrderByDescending(t => t.UploadedAt)).ThenInclude(c => c.StockTearSheetOutputs);

            if (stock == null)
            {
                return NotFound();
            }

            return stock;
        }


        // PUT: api/Stocks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStock(int id, Stock stock)
        {
            if (id != stock.Id)
            {
                return BadRequest();
            }

            _context.Entry(stock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
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

        // POST: api/Stocks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Stock>> PostStock(Stock stock)
        {
            if (_context.Stocks == null)
            {
                return Problem("Entity set 'BreezeDataContext.Stocks'  is null.");
            }
            Stock new_stock = _repo.Add(stock);
            await _repo.SaveChanges();
            return CreatedAtAction("GetStock", new { id = stock.Id }, stock);
        }

        // DELETE: api/Stocks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {

            Stock stock = await _repo.GetById(id);
            if (stock == null)
            {
                return NotFound();
            }

            await _repo.RemoveEntity(id);
            await _repo.SaveChanges();

            return NoContent();
        }

        private bool StockExists(int id)
        {
            return (_context.Stocks?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //   [HttpPost("/tearsheet/{id}")]
        //   public async Task<object> GetTearSheetByStock(int id)

        //   { 



        //}
    }
}
