using Domain.DTOs;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using Domain.Models;

namespace Domain.Services;

public class OrderService: IOrderService
{
    private readonly IOrderGateway _orderGateway;
    private readonly IDishGateway _dishGateway;

    private static Object _lock;

    static OrderService()
    {
        _lock = new();
    }
     
    public OrderService(IOrderGateway orderGateway, IDishGateway dishGateway)
    {
        _orderGateway = orderGateway;
        _dishGateway = dishGateway;
    }
    
    public async Task<long> Create(CreateOrderRequestDto dto)
    {
        var dishIds = dto.Dishes.Select(d => d.Item1).ToArray();
        
        var dishes = await _dishGateway.Get(dishIds);
    
        foreach (var (id, quantity) in dto.Dishes)
        {
            var dish = dishes.FirstOrDefault(d => d.Id == id);
            if (dish == null)
            {
                throw new DishNotFoundException($"Dish with id {id} not found");
            }
            if (dish.Quantity < quantity)
            {
                throw new NotEnoughQuantityException($"Dish with id {id} has only {dish.Quantity} left");
            }
        }
    
        var order = new OrderEntity()
        {
            UserId = dto.UserId,
            SpecialRequests = dto.SpecialIRequests,
            Status = "waiting",
            Dishes = dto.Dishes.Select(d => (new DishEntity()
            {
                Id = d.Item1,
                Price = dishes.First(dish => dish.Id == d.Item1).Price,
                Quantity = d.Item2
            })).ToList()
        };
    
        var orderId = await _orderGateway.Create(order);
        
        await _dishGateway.DecreaseQuantity(dto.Dishes);

        return orderId;
    }

    public async Task<OrderModel> Get(long id)
    {
        var entity = await _orderGateway.Get(id);
        
        if (entity == null)
        {
            throw new OrderNotFoundException($"Order with id {id} not found");
        }

        var order = new OrderModel()
        {
            Id = entity.Id,
            UserId = entity.UserId,
            SpecialRequests = entity.SpecialRequests,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt,
            Dishes = entity.Dishes.Select(d => new DishModel()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                Price = d.Price,
                Quantity = d.Quantity
            }).ToList()
        };

        return order;
    }
}