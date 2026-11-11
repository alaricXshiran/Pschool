namespace Pschool.API.Models;
// A parent or guardian. One parent can have many students.
public class Parent
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    // Optional contact details - these can be left empty.
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? HomeAddress { get; set; }
    public string? Phone1 { get; set; }
    public string? WorkPhone { get; set; }
    public string? HomePhone { get; set; }
    // All children of this parent. Counted to show the "Siblings" column.
    public List<Student> Students { get; set; } = new();
}