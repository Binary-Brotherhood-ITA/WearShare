using System.ComponentModel.DataAnnotations;

namespace Core;

public class LoginRequest
{
    // Email bruges til at identificere medlemmet
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter a valid email")]
    public string Email { get; set; } = string.Empty;

    // Adgangskoden sendes til backend ved login
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}