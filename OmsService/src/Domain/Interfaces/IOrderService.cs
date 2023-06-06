using Domain.DTOs;
using Domain.Models;

namespace Domain.Interfaces;

public interface IOrderService
{
    Task<long> Create(CreateOrderRequestDto dto);
    Task<OrderModel> Get(long id);
}