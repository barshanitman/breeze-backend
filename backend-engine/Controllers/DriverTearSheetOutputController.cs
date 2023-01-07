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
    public class DriverTearSheetOutputController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        public DriverTearSheetOutputController(BreezeDataContext context)
        {
            _context = context;
        }

        // GET: api/DriverTearSheetOutput
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DriverTearSheetOutput>>> GetDriverTearSheetOutputs()
        {
          if (_context.DriverTearSheetOutputs == null)
          {
              return NotFound();
          }
            return await _context.DriverTearSheetOutputs.ToListAsync();
        }

        // GET: api/DriverTearSheetOutput/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DriverTearSheetOutput>> GetDriverTearSheetOutput(int id)
        {
          if (_context.DriverTearSheetOutputs == null)
          {
              return NotFound();
          }
            var driverTearSheetOutput = await _context.DriverTearSheetOutputs.FindAsync(id);

            if (driverTearSheetOutput == null)
            {
                return NotFound();
            }

            return driverTearSheetOutput;
        }

        // PUT: api/DriverTearSheetOutput/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriverTearSheetOutput(int id, DriverTearSheetOutput driverTearSheetOutput)
        {
            if (id != driverTearSheetOutput.Id)
            {
                return BadRequest();
            }

            _context.Entry(driverTearSheetOutput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverTearSheetOutputExists(id))
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

        // POST: api/DriverTearSheetOutput
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DriverTearSheetOutput>> PostDriverTearSheetOutput(DriverTearSheetOutput driverTearSheetOutput)
        {
          if (_context.DriverTearSheetOutputs == null)
          {
              return Problem("Entity set 'BreezeDataContext.DriverTearSheetOutputs'  is null.");
          }
            _context.DriverTearSheetOutputs.Add(driverTearSheetOutput);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDriverTearSheetOutput", new { id = driverTearSheetOutput.Id }, driverTearSheetOutput);
        }

        // DELETE: api/DriverTearSheetOutput/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriverTearSheetOutput(int id)
        {
            if (_context.DriverTearSheetOutputs == null)
            {
                return NotFound();
            }
            var driverTearSheetOutput = await _context.DriverTearSheetOutputs.FindAsync(id);
            if (driverTearSheetOutput == null)
            {
                return NotFound();
            }

            _context.DriverTearSheetOutputs.Remove(driverTearSheetOutput);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverTearSheetOutputExists(int id)
        {
            return (_context.DriverTearSheetOutputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
