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

       // GET /api/students - returns every student.
       [HttpGet]
       public async Task<ActionResult<List<StudentResponse>>> GetStudents()
       {
           var students = await StudentResponses()
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
   }