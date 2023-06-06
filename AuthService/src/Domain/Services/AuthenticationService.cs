using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services;

public class AuthenticationService: IAuthenticationService
{
    private readonly IUserGateway _userGateway;
    private readonly TokenValidationParameters _validationParameters;
    private readonly string _jwtKey;

    public AuthenticationService(IUserGateway userGateway, IConfiguration configuration)
    {
        _userGateway = userGateway;
        _jwtKey = configuration["AuthOptions:JwtKey"]!;
        _validationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = "AuthService",
            ValidateAudience = true,
            ValidAudience = "OmsServices",
            ValidateLifetime = true,
            IssuerSigningKeyResolver = (string _, SecurityToken _, string _,
                TokenValidationParameters _) => new List<SecurityKey>()
            {
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtKey))
            },
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };
    }
    
    public async Task<long> RegisterUser(RegisterRequestDto dto)
    {
        var existingUser = await _userGateway.GetByEmail(dto.Email);
        
        if (existingUser != null)
        {
            throw new EmailOccupiedException();
        }
        
        var hash = HashPassword(dto.Password);
        
        var user = new User
        {
            Email = dto.Email,
            Username = dto.Login,
            PasswordHash = hash,
            Role = dto.Role
        };
        
        return await _userGateway.Create(UserEntity.FromModel(user));
    }

    public async Task<LoginResponseDto> LoginUser(LoginRequestDto dto)
    {
        var entity = await _userGateway.GetByEmail(dto.Email);
        if (entity == null)
        {
            throw new NotFoundException("User not found");
        }
        
        var hash = HashPassword(dto.Password);
        
        if (entity.PasswordHash != hash)
        {
            throw new IncorrectPasswordException();
        }
        
        return new LoginResponseDto()
        {
            User = entity.ToModel(),
            Token = GenerateToken(10, new List<Claim>()
            {
                new(ClaimTypes.Role, entity.Role),
                new(ClaimTypes.NameIdentifier, entity.Id.ToString())
            })
        };
    }
    
    private string GenerateToken(int lifetimeInMinutes, IEnumerable<Claim> claims)
    {
        var jwt = new JwtSecurityToken(
            issuer: _validationParameters.ValidIssuer,
            audience: _validationParameters.ValidAudience,
            claims: claims,
            notBefore: DateTime.Now.ToUniversalTime(),
            expires: DateTime.Now.AddMinutes(lifetimeInMinutes).ToUniversalTime(),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
                SecurityAlgorithms.HmacSha256
            )
        );     
           
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
    
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}