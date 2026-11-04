using System.ComponentModel.DataAnnotations;

namespace Core;

public class RegisterRequest
{
    // Navn på det nye medlem
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    // Medlemmets telefonnummer
    [Required(ErrorMessage = "Phone is required")]
    public string Phone { get; set; } = string.Empty;

    // Medlemmets emailadresse
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = string.Empty;

    // Adgangskoden skal indeholde mindst 8 tegn
    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must contain at least 8 characters")]
    public string Password { get; set; } = string.Empty;
}