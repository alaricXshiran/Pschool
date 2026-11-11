   namespace Pschool.API.Models;

   public class Student
   {
       public int Id { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public DateOnly DateOfBirth { get; set; }

       public int ParentId { get; set; }
       public Parent? Parent { get; set; }
   }