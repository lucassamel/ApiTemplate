using ApiTemplate.Domain.Entities;

namespace ApiTemplate.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
