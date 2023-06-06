using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserGateway
{
    Task<long> Create(UserEntity user);
    Task<UserEntity?> GetByEmail(string email);
    Task<UserEntity?> GetById(long id);
}