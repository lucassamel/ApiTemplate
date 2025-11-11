using ApiTemplate.Application.DTOs;
using ApiTemplate.Domain.Entities;

namespace ApiTemplate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
