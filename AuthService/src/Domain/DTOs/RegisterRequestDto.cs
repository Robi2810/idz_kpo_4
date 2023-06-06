namespace Domain.DTOs;

public class RegisterRequestDto
{
    public string? Email { get; init; }
    public string? Password { get; init; }
    public string? Login { get; init; }
    public string? Role { get; init; }
}