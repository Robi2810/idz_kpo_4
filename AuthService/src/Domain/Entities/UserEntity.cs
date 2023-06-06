using Domain.Models;

namespace Domain.Entities;

public class UserEntity
{
    public long Id { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? Username { get; set; }
    public string? Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public static UserEntity FromModel(User model)
    {
        return new UserEntity
        {
            Email = model.Email,
            PasswordHash = model.PasswordHash,
            Username = model.Username,
            Role = model.Role
        };
    }
    
    public User ToModel()
    {
        return new User
        {
            Id = Id,
            Email = Email,
            PasswordHash = PasswordHash,
            Username = Username,
            Role = Role,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt,
        };
    }
}