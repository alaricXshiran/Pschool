   namespace Pschool.API.Dtos;

   // What a client sends to create or edit a parent.
   // No Id here: the database generates it.
   public class ParentRequest
   {
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string? Username { get; set; }
       public string? Email { get; set; }
       public string? HomeAddress { get; set; }
       public string? Phone1 { get; set; }
       public string? WorkPhone { get; set; }
       public string? HomePhone { get; set; }
   }

   // What the API sends back for a parent.
   public class ParentResponse
   {
       public int Id { get; set; }
       public string FirstName { get; set; } = string.Empty;
       public string LastName { get; set; } = string.Empty;
       public string? Username { get; set; }
       public string? Email { get; set; }
       public string? HomeAddress { get; set; }
       public string? Phone1 { get; set; }
       public string? WorkPhone { get; set; }
       public string? HomePhone { get; set; }

       // Number of students this parent has (the "Siblings" column).
       public int SiblingsCount { get; set; }
   }