using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("menu")]
public class MenuController: ControllerBase
{
    private readonly IDishGateway _dishGateway;

    public MenuController(IDishGateway dishGateway)
    {
        _dishGateway = dishGateway;
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get()
    {
        var dish = await _dishGateway.GetAll();
        return Ok(dish);
    }
}