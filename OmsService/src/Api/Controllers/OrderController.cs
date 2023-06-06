using Api.Requests.Order;
using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;
    
    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var dto = request.ToDto();
        
        var id = await _orderService.Create(dto);
        
        return Ok(id);
    }
    
    [HttpGet]
    [Route("get")]
    public async Task<IActionResult> GetOrder(long id)
    {
        var order = await _orderService.Get(id);
        
        return Ok(order);
    }
    
}