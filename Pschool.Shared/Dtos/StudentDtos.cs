namespace Pschool.Shared.Dtos;

// What a client sends to create or edit a student.
using System.ComponentModel.DataAnnotations;
// No Id here: the database generates it.
public class StudentRequest : IValidatableObject
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(50, ErrorMessage = "First name can be at most 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(50, ErrorMessage = "Last name can be at most 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    public DateOnly DateOfBirth { get; set; }

    // A missing parentId arrives as 0, so anything below 1 means "not chosen".
    [Range(1, int.MaxValue, ErrorMessage = "Please choose a parent.")]
    public int ParentId { get; set; }

    // Custom rules that labels cannot express. Runs after the labels pass.
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        // A missing date arrives as 1 January 0001, so this also catches "not given".
        if (DateOfBirth < new DateOnly(1900, 1, 1))
        {
            yield return new ValidationResult(
                "Date of birth is required.",
                new[] { nameof(DateOfBirth) });
        }
        else if (DateOfBirth > today)
        {
            yield return new ValidationResult(
                "Date of birth cannot be in the future.",
                new[] { nameof(DateOfBirth) });
        }
    }
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