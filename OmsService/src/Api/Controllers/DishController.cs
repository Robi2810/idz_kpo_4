using Api.Requests.DIsh;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("dish")]
[Authorize(Roles="Admin,Manager")]
public class DishController: ControllerBase
{
    private readonly IDishGateway _dishGateway;

    public DishController(IDishGateway dishGateway)
    {
        _dishGateway = dishGateway;
    }

    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> Get(long id)
    {
        var dish = await _dishGateway.Get(id);
        return Ok(dish);
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> Create(CreateDishRequest request)
    {
        var id = await _dishGateway.Create(new DishEntity()
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity
        });
        
        return Ok(id);
    }
    
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult> Update(UpdateDishRequest request)
    {
        var rowsAffected = await _dishGateway.Update(new DishEntity()
        {
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Quantity = request.Quantity
        });
        
        return Ok(rowsAffected);
    }
    
    [HttpDelete]
    [Route("delete")]
    public async Task<IActionResult> Delete(DeleteDishRequest request)
    {
        var rowsAffected = await _dishGateway.Delete(request.Id);
        return Ok(rowsAffected);
    }
    
}