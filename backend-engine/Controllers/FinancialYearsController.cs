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
    public class FinancialYearsController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        private readonly IRepository<FinancialYear> _repo;

        public FinancialYearsController(BreezeDataContext context, IRepository<FinancialYear> repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/FinancialYears
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialYear>>> GetFinancialYears()
        {
            return Ok(await _repo.GetAll());

        }

        // GET: api/FinancialYears/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialYear>> GetFinancialYear(int id)
        {
         
            FinancialYear financialYear = await _repo.GetById(id) ;

            if (financialYear == null)
            {
                return NotFound();
            }

            return financialYear;
        }

        // PUT: api/FinancialYears/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinancialYear(int id, FinancialYear financialYear)
        {
            if (id != financialYear.Id)
            {
                return BadRequest();
            }

            _context.Entry(financialYear).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinancialYearExists(id))
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

        // POST: api/FinancialYears
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinancialYear>> PostFinancialYear(FinancialYear financialYear)
        {
          if (_context.FinancialYears == null)
          {
              return Problem("Entity set 'BreezeDataContext.FinancialYears'  is null.");
          }
            _context.FinancialYears.Add(financialYear);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinancialYear", new { id = financialYear.Id }, financialYear);
        }

        // DELETE: api/FinancialYears/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinancialYear(int id)
        {
            if (_context.FinancialYears == null)
            {
                return NotFound();
            }
            var financialYear = await _context.FinancialYears.FindAsync(id);
            if (financialYear == null)
            {
                return NotFound();
            }

            _context.FinancialYears.Remove(financialYear);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinancialYearExists(int id)
        {
            return (_context.FinancialYears?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

