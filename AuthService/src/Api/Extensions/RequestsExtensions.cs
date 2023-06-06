using Api.Requests;
using Domain.DTOs;

namespace Api.Extensions;

public static class RequestsExtensions
{
    public static RegisterRequestDto ToDto(this RegisterRequest request)
    {
        return new RegisterRequestDto
        {
            Email = request.Email,
            Password = request.Password,
            Login = request.Login,
            Role = request.Role
        };
    }
    
    public static LoginRequestDto ToDto(this LoginRequest request)
    {
        return new LoginRequestDto
        {
            Email = request.Email,
            Password = request.Password
        };
    }
}