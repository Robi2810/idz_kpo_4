using Dapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Infra.Gateways;

public class UserGateway: IUserGateway
{
    private readonly string _connectionString;
    
    public UserGateway(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<long> Create(UserEntity entity)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
INSERT INTO users (username, email, password_hash, role, created_at, updated_at) 
VALUES (@username, @email, @password_hash, @role, now(), now()) returning id", 
            connection);
        var queryParameters = new
        {
            username = entity.Username,
            email = entity.Email,
            password_hash = entity.PasswordHash,
            role = entity.Role
        };
        var id = await connection.QuerySingleAsync<long>(command.CommandText, queryParameters);
        return id;
    }

    public async Task<UserEntity?> GetByEmail(string email)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
SELECT id, username, email, password_hash as PasswordHash, role, created_at as CreatedAt, updated_at as UpdatedAt 
FROM users 
WHERE email = @email", 
            connection);
        var queryParameters = new
        {
            email
        };
        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command.CommandText, queryParameters);
        return entity;
    }

    public async Task<UserEntity?> GetById(long id)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(@"
SELECT id, username, email, password_hash as PasswordHash, role, created_at as CreatedAt, updated_at as UpdatedAt 
FROM users 
WHERE id = @id", 
            connection);
        var queryParameters = new
        {
            id
        };
        var entity = await connection.QuerySingleOrDefaultAsync<UserEntity>(command.CommandText, queryParameters);
        return entity;
    }
}