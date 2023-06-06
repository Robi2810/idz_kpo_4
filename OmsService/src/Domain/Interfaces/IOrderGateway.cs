using Domain.Entities;

namespace Domain.Interfaces;

public interface IOrderGateway
{
    Task<OrderEntity> Get(long id);
    Task<IEnumerable<OrderEntity>> GetPending(long limit);
    Task<long> SetStatus(long id, string status);
    Task<long> Create(OrderEntity order);
}