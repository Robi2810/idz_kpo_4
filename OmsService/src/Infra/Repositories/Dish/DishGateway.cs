using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Domain.Entities;
using Domain.Interfaces;

namespace Infra.Repositories.Dish;

public class DishGateway: IDishGateway
{
    private readonly string _connectionString;
    
    public DishGateway(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<long> Create(DishEntity dishEntity)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
INSERT INTO dish (name, description, price, quantity) 
VALUES (@name, @description, @price, @quantity) 
returning id", 
            connection);
        var queryParameters = new
        {
            name = dishEntity.Name,
            description = dishEntity.Description,
            price = dishEntity.Price,
            quantity = dishEntity.Quantity
        };
        return await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
    }

    public async Task<DishEntity> Get(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand($@"
SELECT *
FROM dish 
WHERE id = @id", 
            connection);
        var queryParameters = new
        {
            id
        };
        return await connection.QuerySingleAsync<DishEntity>(command.CommandText, queryParameters);
    }

    public async Task<List<DishEntity>> GetAll()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
SELECT * 
FROM dish", 
            connection);
        return (await connection.QueryAsync<DishEntity>(command.CommandText)).ToList();
    }

    public async Task<List<DishEntity>> Get(long[] ids)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
SELECT *
FROM dish 
WHERE id = any(@ids)", 
            connection);
        var queryParameters = new
        {
            ids = ids
        };
        return (await connection.QueryAsync<DishEntity>(command.CommandText, queryParameters)).ToList();
    }

    public async Task<long> Update(DishEntity dishEntity)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
UPDATE dish 
SET name = @name, description = @description, price = @price, quantity = @quantity 
WHERE id = @id", 
            connection);
        var queryParameters = new
        {
            id = dishEntity.Id,
            name = dishEntity.Name,
            description = dishEntity.Description,
            price = dishEntity.Price,
            quantity = dishEntity.Quantity
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task<long> Delete(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand("DELETE FROM dish WHERE id = @id", connection);
        var queryParameters = new
        {
            id
        };
        return await connection.ExecuteAsync(command.CommandText, queryParameters);
    }

    public async Task<long> DecreaseQuantity(IEnumerable<(long, int)> dishes)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
UPDATE dish 
SET quantity = quantity - @quantity
WHERE id = @id", connection);
        var rowsAffected = 0;
        foreach (var (id, quantity) in dishes)
        {
            var queryParameters = new
            {
                id,
                quantity
            };
            rowsAffected += await connection.ExecuteAsync(command.CommandText, queryParameters);
        }

        return rowsAffected;
    }
}