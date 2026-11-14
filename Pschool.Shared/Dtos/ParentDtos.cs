using System.ComponentModel.DataAnnotations;
namespace Pschool.Shared.Dtos;

// What a client sends to create or edit a parent.
// No Id here: the database generates it.
// What a client sends to create or edit a parent.
// No Id here: the database generates it.
public class ParentRequest
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name can be at most 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name can be at most 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Username can be at most 50 characters.")]
    public string? Username { get; set; }

    [EmailAddress(ErrorMessage = "Email is not a valid email address.")]
    [StringLength(100, ErrorMessage = "Email can be at most 100 characters.")]
    public string? Email { get; set; }

    [StringLength(200, ErrorMessage = "Home address can be at most 200 characters.")]
    public string? HomeAddress { get; set; }

    [Phone(ErrorMessage = "Phone 1 is not a valid phone number.")]
    [StringLength(20, ErrorMessage = "Phone 1 can be at most 20 characters.")]
    public string? Phone1 { get; set; }

    [Phone(ErrorMessage = "Work phone is not a valid phone number.")]
    [StringLength(20, ErrorMessage = "Work phone can be at most 20 characters.")]
    public string? WorkPhone { get; set; }

    [Phone(ErrorMessage = "Home phone is not a valid phone number.")]
    [StringLength(20, ErrorMessage = "Home phone can be at most 20 characters.")]
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

// Minimal parent details for dropdown lists.
public class ParentLookup
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
}