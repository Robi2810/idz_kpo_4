using System.ComponentModel.DataAnnotations;
using Domain.DTOs;

namespace Api.Requests.Order;

public class CreateOrderRequest
{
    [Required]
    public long UserId { get; set; }
    public string? SpecialIRequests { get; set; }
    [Required]
    public Dictionary<long, int> DishQuantities { get; set; }
    
    public CreateOrderRequestDto ToDto()
    {
        return new CreateOrderRequestDto
        {
            UserId = UserId,
            SpecialIRequests = SpecialIRequests,
            Dishes = DishQuantities.Select(pair => (pair.Key, pair.Value)).ToList()
        };
    }
}