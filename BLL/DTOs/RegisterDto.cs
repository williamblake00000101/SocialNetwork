using System.ComponentModel.DataAnnotations;

namespace BLL.DTOs;

public class RegisterDto
{
    [Required] public string UserName { get; set; }
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Gender { get; set; }
    [Required] public DateOnly? DateOfBirth { get; set; }
    [Required] public string City { get; set; }
    [Required] public string Country { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 6)]
    public string Password { get; set; }
}