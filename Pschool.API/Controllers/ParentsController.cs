using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pschool.API.Data;
using Pschool.Shared.Dtos;
using Pschool.API.Models;

namespace Pschool.API.Controllers;

// Handles all web requests about parents. Address: /api/parents
[ApiController]
[Route("api/[controller]")]
public class ParentsController : ControllerBase
{
    // Our connection to the database.
    private readonly PschoolDbContext _context;

    public ParentsController(PschoolDbContext context)
    {
        _context = context;
    }

    // Shared recipe: turn Parent rows into ParentResponse objects,
    // letting the database count each parent's students.
    private IQueryable<ParentResponse> ParentResponses()
    {
        return _context.Parents.Select(parent => new ParentResponse
        {
            Id = parent.Id,
            FirstName = parent.FirstName,
            LastName = parent.LastName,
            Username = parent.Username,
            Email = parent.Email,
            HomeAddress = parent.HomeAddress,
            Phone1 = parent.Phone1,
            WorkPhone = parent.WorkPhone,
            HomePhone = parent.HomePhone,
            SiblingsCount = parent.Students.Count
        });
    }

    // GET /api/parents?page=1&pageSize=10&search=chamila
    // Returns one page of parents, optionally filtered by a search word.
    [HttpGet]
    public async Task<ActionResult<PagedResult<ParentResponse>>> GetParents(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null)
    {
        // Keep the values sensible: page starts at 1, at most 100 rows per page.
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = ParentResponses();

        // Search in names, username and email, ignoring upper/lower case.
        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLower();

            query = query.Where(item =>
                item.FirstName.ToLower().Contains(term) ||
                item.LastName.ToLower().Contains(term) ||
                (item.Username != null && item.Username.ToLower().Contains(term)) ||
                (item.Email != null && item.Email.ToLower().Contains(term)));
        }

        // Count all matches before cutting out one page.
        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(item => item.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new PagedResult<ParentResponse>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }

    // GET /api/parents/5 - returns the one parent with that Id.
    [HttpGet("{id}")]
    public async Task<ActionResult<ParentResponse>> GetParent(int id)
    {
        var parent = await ParentResponses()
            .FirstOrDefaultAsync(item => item.Id == id);

        // No parent with that Id: answer "404 Not Found".
        if (parent == null)
        {
            return NotFound();
        }

        return Ok(parent);
    }

    // POST /api/parents - saves a new parent sent in the request.
    [HttpPost]
    public async Task<ActionResult<ParentResponse>> CreateParent(ParentRequest request)
    {
        // Copy the allowed fields into a new Parent. The Id is left for the database.
        var parent = new Parent
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            Email = request.Email,
            HomeAddress = request.HomeAddress,
            Phone1 = request.Phone1,
            WorkPhone = request.WorkPhone,
            HomePhone = request.HomePhone
        };

        _context.Parents.Add(parent);
        await _context.SaveChangesAsync();

        // Read it back in reply format and answer "201 Created".
        var response = await ParentResponses()
            .FirstAsync(item => item.Id == parent.Id);

        return CreatedAtAction(nameof(GetParent), new { id = parent.Id }, response);
    }

    // PUT /api/parents/5 - replaces the details of an existing parent.
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateParent(int id, ParentRequest request)
    {
        var parent = await _context.Parents.FindAsync(id);

        if (parent == null)
        {
            return NotFound();
        }

        // Copy the new values over the old ones.
        parent.FirstName = request.FirstName;
        parent.LastName = request.LastName;
        parent.Username = request.Username;
        parent.Email = request.Email;
        parent.HomeAddress = request.HomeAddress;
        parent.Phone1 = request.Phone1;
        parent.WorkPhone = request.WorkPhone;
        parent.HomePhone = request.HomePhone;

        await _context.SaveChangesAsync();

        // "204 No Content": it worked, and there is nothing to send back.
        return NoContent();
    }

    // DELETE /api/parents/5 - removes a parent who has no students.
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteParent(int id)
    {
        var parent = await _context.Parents.FindAsync(id);

        if (parent == null)
        {
            return NotFound();
        }

        // Every student must have a parent, so refuse while students remain.
        var hasStudents = await _context.Students.AnyAsync(student => student.ParentId == id);

        if (hasStudents)
        {
            return Conflict("This parent still has students. Delete or reassign the students first.");
        }

        _context.Parents.Remove(parent);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // GET /api/parents/lookup - every parent's Id and name, sorted by name, for dropdowns.
    [HttpGet("lookup")]
    public async Task<ActionResult<List<ParentLookup>>> GetParentLookup()
    {
        var parents = await _context.Parents
            .OrderBy(parent => parent.FirstName)
            .ThenBy(parent => parent.LastName)
            .Select(parent => new ParentLookup
            {
                Id = parent.Id,
                FullName = parent.FirstName + " " + parent.LastName
            })
            .ToListAsync();

        return Ok(parents);
    }
}