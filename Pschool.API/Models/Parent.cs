namespace Pschool.API.Models;

public class Parent
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
}