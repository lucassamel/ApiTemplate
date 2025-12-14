using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiTemplate.Application.Services 
{

    public class AuthService(IRepository<User> userRepository, 
        ITokenService tokenService, IUserRepository userRepository2,
        IRepository<Address> addressRepository) : IAuthService
    {
        private readonly IRepository<User> _userRepository = userRepository;
        private readonly IRepository<Address> _addressRepository = addressRepository;
        private readonly IUserRepository _userRepository2 = userRepository2;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            try
            {               
                var user = await _userRepository2.GetUserByUsername(loginDto.Username);

                if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                {
                    throw new UnauthorizedAccessException("Credenciais inválidas");
                }

                var token = _tokenService.GenerateToken(user);

                return new AuthResponseDto
                {
                    Token = token,
                    Username = user.Username,
                    Email = user.Email,
                    ExpiresAt = DateTime.UtcNow.AddHours(2)
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao efetuar login", ex);
            }    
            
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var users = await _userRepository.GetAllAsync();

            if (users.Any(u => u.Username == registerDto.Username))
            {
                throw new InvalidOperationException("Username já existe");
            }

            if (users.Any(u => u.Email == registerDto.Email))
            {
                throw new InvalidOperationException("Email já cadastrado");
            }

            var user = new User
            {
                Username = registerDto.Username.Trim().ToLower(),
                Email = registerDto.Email.Trim().ToLower(),
                PasswordHash = HashPassword(registerDto.Password),
                Role = registerDto.Role!.ToLower(),
                CreatedAt = DateTime.UtcNow
            };

            var userCreated = await _userRepository.AddAsync(user);

            var address = new Address
            {
                Bairro = registerDto.Address.Bairro,
                Cep = registerDto.Address.Cep,
                Complemento = registerDto.Address.Complemento,
                Localidade = registerDto.Address.Localidade,
                Uf = registerDto.Address.Uf,
                Logradouro = registerDto.Address.Logradouro,
                UserId = userCreated.Id
            };

            await _addressRepository.AddAsync(address);

            var token = _tokenService.GenerateToken(user);

            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Email = user.Email,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            var users = await _userRepository.GetAllAsync();
            return users.FirstOrDefault(u => u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }
}
