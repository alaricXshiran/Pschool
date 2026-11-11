   using Microsoft.AspNetCore.Mvc;
   using Microsoft.EntityFrameworkCore;
   using Pschool.API.Data;
   using Pschool.API.Models;

   namespace Pschool.API.Controllers;

   // Handles all web requests about parents. Address: /api/parents
   [ApiController]
   [Route("api/[controller]")]
   public class ParentsController : ControllerBase
   {
       // Our connection to the database.
       private readonly PschoolDbContext _context;

       // The app hands us the database context when it creates this controller.
       public ParentsController(PschoolDbContext context)
       {
           _context = context;
       }

       // GET /api/parents - returns every parent in the database.
       [HttpGet]
       public async Task<ActionResult<List<Parent>>> GetParents()
       {
           var parents = await _context.Parents.ToListAsync();
           return Ok(parents);
       }
   }