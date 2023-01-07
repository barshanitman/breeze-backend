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
    public class TearSheetOutputsController : ControllerBase
    {
        private readonly BreezeDataContext _context;

        private readonly IRepository<TearSheetOutput> _repo;

        public TearSheetOutputsController(BreezeDataContext context, IRepository<TearSheetOutput>repo)
        {
            _context = context;
            _repo = repo;
        }

        // GET: api/TearSheetOutputs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TearSheetOutput>>> GetTearSheetOutputs()
        {
         
            return Ok(await _repo.GetAll()) ;
        }

        // GET: api/TearSheetOutputs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TearSheetOutput>> GetTearSheetOutput(int id)
        {
         
            TearSheetOutput tearSheetOutput = await _repo.GetById(id);

            if (tearSheetOutput == null)
            {
                return NotFound();
            }

            return tearSheetOutput;
        }

        // PUT: api/TearSheetOutputs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTearSheetOutput(int id, TearSheetOutput tearSheetOutput)
        {
            if (id != tearSheetOutput.Id)
            {
                return BadRequest();
            }

            _context.Entry(tearSheetOutput).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TearSheetOutputExists(id))
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

        // POST: api/TearSheetOutputs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TearSheetOutput>> PostTearSheetOutput(TearSheetOutput tearSheetOutput)
        {
          if (_context.TearSheetOutputs == null)
          {
              return Problem("Entity set 'BreezeDataContext.TearSheetOutputs'  is null.");
          }
            _repo.Add(tearSheetOutput);
            await _repo.SaveChanges();

            return CreatedAtAction("GetTearSheetOutput", new { id = tearSheetOutput.Id }, tearSheetOutput);
        }

        // DELETE: api/TearSheetOutputs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTearSheetOutput(int id)
        {
            await _repo.RemoveEntity(id);
            return Ok();
        }

        private bool TearSheetOutputExists(int id)
        {
            return (_context.TearSheetOutputs?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
