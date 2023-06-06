using System.ComponentModel.DataAnnotations;

namespace Api.Requests;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string? Email { get; } = null!;
    [Required]
    public string? Password { get; } = null!;
    [Required]
    public string? Login { get; } = null!;
    [Required]
    public string? Role { get; } = null!;
}