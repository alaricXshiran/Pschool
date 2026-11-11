namespace Pschool.API.Models;

public class Student
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    // Foreign key: every student must belong to a parent.
    public int ParentId { get; set; }
    // Lets us reach the parent's details from a student.
    public Parent? Parent { get; set; }
}