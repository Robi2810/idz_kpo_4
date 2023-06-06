using System.IdentityModel.Tokens.Jwt;
using Api.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Extensions;
using Domain.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : Controller
{
    private readonly IAuthenticationService _authenticationService;
    
    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var dto = request.ToDto();
        
        await _authenticationService.RegisterUser(dto);
        
        return Ok();
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var dto = request.ToDto();
        
        var responseDto = await _authenticationService.LoginUser(dto);
        
        return Ok(responseDto);
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [Route("get")]
    public async Task<IActionResult> GetUser(long id, [FromServices] IUserGateway userGateway)
    {
        var entity = await userGateway.GetById(id);
        
        if (entity == null)
        {
            return NotFound();
        }
        
        return Ok(entity.ToModel());
    }
    
}