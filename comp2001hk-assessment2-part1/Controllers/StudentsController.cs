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
    public class StudentsController : ControllerBase
    {
        private readonly comp2001hk_assessment2_part1Context _context;

        public StudentsController(comp2001hk_assessment2_part1Context context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet(Name = "GetStudents")]
        [SwaggerOperation(Summary = "Get all students information")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(IEnumerable<Student>))]
        public async Task<ActionResult<IEnumerable<Object>>> GetStudents()
        {

            var result = await _context.Students
                .Select(s => new
                {
                    s.Student_id,
                    s.Name,
                    Programmes = s.Programmes.Select(
                        p => new
                        {
                            p.Id,
                            p.Programme_code,
                            p.Programme_title
                        })
                    .ToList()
                }).ToListAsync();

            return result;
        }

        // GET: api/Students/5
        [HttpGet("{id}", Name = "GetStudent")]
        [SwaggerOperation(Summary = "Get students information by specify ID")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(IEnumerable<Student>))]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Student not found")]
        public async Task<ActionResult<Object>> GetStudent(int id)
        {
            var student = await _context.Students
                .Where(s => s.Student_id == id)
                .Select(s => new
                {
                    s.Student_id,
                    s.Name,
                    Programmes = s.Programmes.Select(
                        p => new
                        {
                            p.Id,
                            p.Programme_code,
                            p.Programme_title
                        })
                }).FirstOrDefaultAsync();

            if (student == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Student not found" });
            }

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}", Name = "PutStudent")]
        [SwaggerOperation(Summary = "Update an existing student")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Success")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid ID")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Student not found")]
        public async Task<IActionResult> PutStudent(int id, StudentDto student)
        {
            if (id != student.Student_id)
            {
                //return BadRequest();
                return StatusCode(StatusCodes.Status400BadRequest, new Response
                { Status = "Success", Message = "Invalid ID" });
            }

            var dataBefore = await _context.Students
                .Include(p => p.Programmes)
                .FirstOrDefaultAsync(s => s.Student_id == id);

            if (dataBefore == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Student not found" });
            }

            dataBefore.Name = student.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    //return NotFound();
                    return StatusCode(StatusCodes.Status404NotFound, new Response
                    { Status = "Error", Message = "Student not found" });
                }
                else
                {
                    throw;
                }
            }
            
            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Student updated" });
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Admin")]
        [HttpPost(Name = "PostStudent")]
        [SwaggerOperation(Summary = "Register a new statudent")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Student))]
        public async Task<ActionResult<Student>> PostStudent(StudentDto student)
        {
            var studentToCreate = new Student
            {
                Name = student.Name
            };

            _context.Students.Add(studentToCreate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            student.Student_id = studentToCreate.Student_id;

            return CreatedAtAction("GetStudent", new { id = student.Student_id }, student);
        }

        // POST: api/Students/5/Programme/2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}/Programme/{code}", Name = "StudentRegisterProgramme")]
        [SwaggerOperation(Summary = "Student enroll a programme")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Registed programme successfully")]
        public async Task<ActionResult<Student>> StudentRegisterProgramme(int id, int code)
        {
                Student student = new Student { Student_id = id };
                _context.Students.Add(student);
                _context.Students.Attach(student);

                Programme programme = new Programme { Id = code };
                _context.Programmes.Add(programme);
                _context.Programmes.Attach(programme);

                student.Programmes = new List<Programme>();
                student.Programmes.Add(programme);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Registed programme successfully" });
        }

        // DELETE: api/Students/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteStudent")]
        [SwaggerOperation(Summary = "Delete student")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Request Successful")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Student not found" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Student deleted" });
        }

        // DELETE: api/Students/5/Programme/1
        [HttpDelete("{id}/Programme/{code}", Name = "DeleteStudentFromPgramme")]
        [SwaggerOperation(Summary = "Remove student from programme")]
        [SwaggerResponse(StatusCodes.Status204NoContent, "Deregisted programme")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "Data not found")]
        public async Task<IActionResult> DeleteStudentFromPgramme(int id, int code)
        {
            Student student = _context.Students
                .Include(x => x.Programmes)
                .Single(x => x.Student_id == id);

            _context.Students.Attach(student);

            Programme programmeToDelete = student.Programmes.FirstOrDefault(x => x.Id == code);
            if (programmeToDelete == null)
            {
                //return NotFound();
                return StatusCode(StatusCodes.Status404NotFound, new Response
                { Status = "Error", Message = "Data not found" });
            }

            student.Programmes.Remove(programmeToDelete);
            await _context.SaveChangesAsync();


            //return NoContent();
            return StatusCode(StatusCodes.Status204NoContent, new Response
            { Status = "Success", Message = "Deregisted programme successfully" });
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Student_id == id);
        }
    }
}
