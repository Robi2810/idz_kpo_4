using Domain.Models;

namespace Domain.DTOs;

public class LoginResponseDto
{
    public User User { get; set; }
    public string? Token { get; set; }
}