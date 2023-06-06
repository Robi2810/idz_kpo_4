using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Domain.Entities;
using Domain.Interfaces;

namespace Infra.Repositories.Order;

public class OrderGateway: IOrderGateway
{
    private readonly string _connectionString;
    
    public OrderGateway(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }
    
    public async Task<OrderEntity> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
select o.id, o.user_id as UserId, o.special_requests as SpecialRequests, o.status, o.created_at as CreatedAt, o.updated_at as UpdatedAt, d.id as Id, d.name as Name, d.description as Description, od.price as Price, od.quantity as Quantity 
from ""order"" o 
    join order_dish od on o.id = od.order_id 
    join dish d on d.id = od.dish_id 
where o.id = @id", 
            connection);
        var queryParameters = new
        {
            id
        };
        var order = await connection.QueryAsync<OrderEntity, DishEntity, OrderEntity>(command.CommandText, (order, dish) =>
        {
            order.Dishes.Add(dish);
            return order;
        }, queryParameters, splitOn: "Id");

        var result = order.GroupBy(order => order.Id).Select(g =>
        {
            var groupedOrder = g.First();
            groupedOrder.Dishes = g.SelectMany(order => order.Dishes).ToList();
            return groupedOrder;
        });
        
        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<OrderEntity>> GetPending(long limit)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
select id, user_id as UserId, status, special_requests as SpecialRequests, created_at as CreatedAt, updated_at as UpdatedAt 
from ""order""
where status = 'waiting' order by created_at limit @limit", 
            connection);
        var queryParameters = new
        {
            limit
        };
        var orders = await connection.QueryAsync<OrderEntity>(command.CommandText, queryParameters);
        return orders;
    }

    public async Task<long> SetStatus(long id, string status)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
update ""order"" 
set status = @status, updated_at = now()
where id = @id", connection);
        var queryParameters = new
        {
            id,
            status
        };
        var result = await connection.ExecuteAsync(command.CommandText, queryParameters);
        return result;
    }

    public async Task<long> Create(OrderEntity order)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
INSERT INTO ""order"" (user_id, special_requests, status, created_at, updated_at)
VALUES (@userId, @specialRequests, @status, now(), now()) 
returning id",
            connection);
        
        var queryParameters = new
        {
            userId = order.UserId,
            specialRequests = order.SpecialRequests,
            status = order.Status
        };
        
        var orderId = await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
        
        await using var command2 = new NpgsqlCommand(@"
INSERT INTO order_dish (order_id, dish_id, price, quantity) 
VALUES (@orderId, @dishId, @price, @quantity)",
            connection);
        
        foreach (var dish in order.Dishes)
        {
            var queryParameters2 = new
            {
                orderId,
                dishId = dish.Id,
                price = dish.Price,
                quantity = dish.Quantity
            };
            
            await connection.ExecuteAsync(command2.CommandText, queryParameters2);
        }
        
        return orderId;
    }
}