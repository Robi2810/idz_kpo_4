namespace Domain.Models;

public class User
{
    public long Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}