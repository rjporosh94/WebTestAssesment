using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTestAssesment.Models;

namespace WebTestAssesment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarksApiController : ControllerBase
    {
        private readonly TestAssesmentDbContext _context;

        public MarksApiController(TestAssesmentDbContext context)
        {
            _context = context;
        }

        // GET: api/MarksApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mark>>> GetMarks()
        {
          if (_context.Marks == null)
          {
              return NotFound();
          }
            return await _context.Marks.ToListAsync();
        }

        // GET: api/MarksApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mark>> GetMark(int id)
        {
          if (_context.Marks == null)
          {
              return NotFound();
          }
            var mark = await _context.Marks.FindAsync(id);

            if (mark == null)
            {
                return NotFound();
            }

            return mark;
        }

        // PUT: api/MarksApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMark(int id, Mark mark)
        {
            if (id != mark.Id)
            {
                return BadRequest();
            }

            _context.Entry(mark).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarkExists(id))
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

        // POST: api/MarksApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mark>> PostMark(Mark mark)
        {
          if (_context.Marks == null)
          {
              return Problem("Entity set 'TestAssesmentDbContext.Marks'  is null.");
          }
            _context.Marks.Add(mark);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MarkExists(mark.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMark", new { id = mark.Id }, mark);
        }

        // DELETE: api/MarksApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMark(int id)
        {
            if (_context.Marks == null)
            {
                return NotFound();
            }
            var mark = await _context.Marks.FindAsync(id);
            if (mark == null)
            {
                return NotFound();
            }

            _context.Marks.Remove(mark);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MarkExists(int id)
        {
            return (_context.Marks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
