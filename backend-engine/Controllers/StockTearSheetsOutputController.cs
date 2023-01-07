using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend_engine.Models;
using backend_engine.Repositories;

namespace backend_engine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockTearSheetsOutputController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        private readonly IRepository<StockTearSheetOutput> _repo;

        public StockTearSheetsOutputController(BreezeDataContext context, IRepository<StockTearSheetOutput>repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/StockTearSheetsOutput
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockTearSheetOutput>>> GetStockTearSheetOutputs()
        {

            return Ok(await _repo.GetAll());
        }

        // GET: api/StockTearSheetsOutput/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockTearSheetOutput>> GetStockTearSheetOutput(int id)
        {
         
            StockTearSheetOutput stockTearSheetOutput = await _repo.GetById(id);

            if (stockTearSheetOutput == null)
            {
                return NotFound();
            }

            return stockTearSheetOutput;
        }

        [HttpGet("/StockUpload/{id}")]
        public async Task<ActionResult<StockTearSheetOutput>> GetByStock(int id)
        {
         
            StockTearSheetOutput stockTearSheetOutput = await _repo.FindByCondition(a => a.StockUploadId == id);

            if (stockTearSheetOutput == null)
            {
                return NotFound();
            }

            return stockTearSheetOutput;
        }


        // PUT: api/StockTearSheetsOutput/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockTearSheetOutput(int id, StockTearSheetOutput stockTearSheetOutput)
        {
            if (id != stockTearSheetOutput.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockTearSheetOutput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockTearSheetOutputExists(id))
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

        // POST: api/StockTearSheetsOutput
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StockTearSheetOutput>> PostStockTearSheetOutput(StockTearSheetOutput stockTearSheetOutput)
        {
          if (_context.StockTearSheetOutputs == null)
          {
              return Problem("Entity set 'BreezeDataContext.StockTearSheetOutputs'  is null.");
          }
            _context.StockTearSheetOutputs.Add(stockTearSheetOutput);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockTearSheetOutput", new { id = stockTearSheetOutput.Id }, stockTearSheetOutput);
        }

        // DELETE: api/StockTearSheetsOutput/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockTearSheetOutput(int id)
        {

            await _repo.RemoveEntity(id);
            return NoContent();
        }

        private bool StockTearSheetOutputExists(int id)
        {
            return (_context.StockTearSheetOutputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
