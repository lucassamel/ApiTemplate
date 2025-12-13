using ApiTemplate.Domain.Entities;

namespace ApiTemplate.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserByUsername(string username);
    }
}
