using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pschool.API.Data;
using Pschool.API.Dtos;
using Pschool.API.Models;

namespace Pschool.API.Controllers;

// Handles all web requests about students. Address: /api/students
[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly PschoolDbContext _context;

    public StudentsController(PschoolDbContext context)
    {
        _context = context;
    }

    // Builds the student query once, so every endpoint returns the same fields.
    // including the parent's full name.
    private IQueryable<StudentResponse> StudentResponses()
    {
        return _context.Students.Select(student => new StudentResponse
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            DateOfBirth = student.DateOfBirth,
            ParentId = student.ParentId,
            ParentName = student.Parent!.FirstName + " " + student.Parent.LastName
        });
    }

    // GET /api/students              - returns every student.
    // GET /api/students?parentId=1   - returns only that parent's students.
    [HttpGet]
    public async Task<ActionResult<List<StudentResponse>>> GetStudents([FromQuery] int? parentId)
    {
        var query = StudentResponses();

        // Only filter when a parent was chosen.
        if (parentId.HasValue)
        {
            query = query.Where(item => item.ParentId == parentId.Value);
        }

        var students = await query
            .OrderBy(item => item.Id)
            .ToListAsync();

        return Ok(students);
    }

    // GET /api/students/5 - returns the one student with that Id.
    [HttpGet("{id}")]
    public async Task<ActionResult<StudentResponse>> GetStudent(int id)
    {
        var student = await StudentResponses()
            .FirstOrDefaultAsync(item => item.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        return Ok(student);
    }

    // POST /api/students - creates a new student sent in the request.
    [HttpPost]
    public async Task<ActionResult<StudentResponse>> CreateStudent(StudentRequest request)
    {
        // Every student must belong to an existing parent.
        var parentExists = await _context.Parents.AnyAsync(parent => parent.Id == request.ParentId);

        if (!parentExists)
        {
            return BadRequest($"Parent with Id {request.ParentId} does not exist.");
        }

        // Copy the allowed fields into a new Student. The Id is left for the database.
        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            ParentId = request.ParentId
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        // Read it back in reply format (with the parent's name) and answer "201 Created".
        var response = await StudentResponses()
            .FirstAsync(item => item.Id == student.Id);

        return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, response);
    }
    // PUT /api/students/5 - replaces the details of an existing student.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(int id, StudentRequest request)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        // The (possibly new) parent must exist.
        var parentExists = await _context.Parents.AnyAsync(parent => parent.Id == request.ParentId);

        if (!parentExists)
        {
            return BadRequest($"Parent with Id {request.ParentId} does not exist.");
        }

        // Copy the new values over the old ones.
        student.FirstName = request.FirstName;
        student.LastName = request.LastName;
        student.DateOfBirth = request.DateOfBirth;
        student.ParentId = request.ParentId;

        await _context.SaveChangesAsync();

        // "204 No Content": it worked, and there is nothing to send back.
        return NoContent();
    }
    // DELETE /api/students/5 - removes a student.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}