using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces;

public interface IDishGateway
{
    Task<long> Create(DishEntity dishEntity);
    Task<DishEntity> Get(long id);
    Task<List<DishEntity>> GetAll();
    Task<List<DishEntity>> Get(long[] ids);
    Task<long> Update(DishEntity dishEntity);
    Task<long> Delete(long id);
    Task<long> DecreaseQuantity(IEnumerable<(long, int)> dishes);
}