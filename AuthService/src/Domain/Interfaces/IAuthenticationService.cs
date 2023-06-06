using Domain.DTOs;
using Domain.Models;

namespace Domain.Interfaces;

public interface IAuthenticationService
{
    Task<long> RegisterUser(RegisterRequestDto dto);
    Task<LoginResponseDto> LoginUser(LoginRequestDto dto);
}