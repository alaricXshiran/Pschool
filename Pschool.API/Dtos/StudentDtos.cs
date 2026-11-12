   namespace Pschool.API.Dtos;

   // What a client sends to create or edit a student.
   public class StudentRequest
   {
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public DateOnly DateOfBirth { get; set; }

       // Which parent this student belongs to.
       public int ParentId { get; set; }
   }
   

   // What the API sends back for a student.
   public class StudentResponse
   {
       public int Id { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public DateOnly DateOfBirth { get; set; }
       public int ParentId { get; set; }

       // The parent's full name, so the students table can show it directly.
       public string ParentName { get; set; } = string.Empty;
   }