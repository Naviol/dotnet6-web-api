#nullable disable
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using comp2001hk_assessment2_part1.Data;
using comp2001hk_assessment2_part1.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace comp2001hk_assessment2_part1.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ProgrammesController : ControllerBase
    {
        private readonly comp2001hk_assessment2_part1Context _context;

        public ProgrammesController(comp2001hk_assessment2_part1Context context)
        {
            _context = context;
        }

        // GET: api/Programmes
        [HttpGet(Name = "GetProgrammes")]
        [SwaggerOperation(Summary = "List all programmes with registed students")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(IEnumerable<Programme>))]
        public async Task<ActionResult<IEnumerable<Object>>> GetProgrammes()
        {

            var result = await _context.Programmes
                .Select(p => new
                {
                    p.Id,
                    p.Programme_code,
                    p.Programme_title,
                    Students = p.Students.Select(
                        s => new
                        {
                            s.Student_id,
                            s.Name
                        })
                    .ToList()
                }).ToListAsync();

            return result;
        }

        // GET: api/Programmes/5
        [HttpGet("{id}", Name = "GetProgramme")]
        [SwaggerOperation(Summary = "List programme by specify ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Programme))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Programme not found")]
        public async Task<ActionResult<Object>> GetProgramme(int id)
        {
            var programme = await _context.Programmes
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Programme_code,
                    p.Programme_title,
                    Students = p.Students.Select(
                        s => new
                        {
                            s.Student_id,
                            s.Name
                        })
                }).FirstOrDefaultAsync();

            if (programme == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Programme not found" });
            }

            return programme;
        }

        // PUT: api/Programmes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "PutProgramme")]
        [SwaggerOperation(Summary = "Update an existing programme")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Success")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Programme not found")]
        public async Task<IActionResult> PutProgramme(int id, ProgrammeDto programme)
        {

            if (id != programme.Id)
            {
                //return BadRequest();
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                { Status = "Error", Message = "Invalid ID"});
            }

            var dataBefore = await _context.Programmes
                .Include(p => p.Students)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (dataBefore == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Programme not found" });
            }

            dataBefore.Programme_code = programme.Programme_code;
            dataBefore.Programme_title = programme.Programme_title;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProgrammeExists(id))
                {
                    //return NotFound();
                    return StatusCode(StatusCodes.Status404NotFound, new Response
                    { Status = "Error", Message = "Programme not found" });
                }
                else
                {
                    throw;
                }
            }

            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Programme updated" });
        }


        // POST: api/Programmes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "PostProgramme")]
        [SwaggerOperation(Summary = "Create a new programme")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Request Successful")]
        public async Task<ActionResult<Programme>> PostProgramme(ProgrammeDto programme)
        {

            var programmeToCreate = new Programme
            {
                Programme_code=programme.Programme_code,
                Programme_title=programme.Programme_title
            };

            _context.Programmes.Add(programmeToCreate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            programme.Id = programmeToCreate.Id;

            return CreatedAtAction("GetProgramme", new { id = programme.Id}, programme);
        }


        // DELETE: api/Programmes/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteProgramme")]
        [SwaggerOperation(Summary = "Delete programme")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Programme deleted")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Programme not found")]
        public async Task<IActionResult> DeleteProgramme(int id)
        {
            var programme = await _context.Programmes.FindAsync(id);
            if (programme == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Programme not found" });
            }

            _context.Programmes.Remove(programme);
            await _context.SaveChangesAsync();

            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Programme deleted" });
        }

        private bool ProgrammeExists(int id)
        {
            return _context.Programmes.Any(e => e.Id == id);
        }
    }
}
