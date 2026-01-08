using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Application.Services;
using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;
using FluentAssertions;
using Moq;
using System.Text;

namespace ApiTemplate.UnitTests.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly Mock<IUserRepository> _mockUserRepository2;
        private readonly Mock<IRepository<Address>> _mockAddressRepository;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockTokenService = new Mock<ITokenService>();
            _authService = new AuthService(_mockUserRepository.Object, _mockTokenService.Object
            , _mockUserRepository2.Object, _mockAddressRepository.Object);
        }

        [Fact]
        public async Task RegisterAsync_WithValidData_ShouldCreateUserAndReturnToken()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Test123!",
                Role = "User"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User>());

            _mockUserRepository
                .Setup(r => r.AddAsync(It.IsAny<User>()))
                .ReturnsAsync((User user) => user);
            
            

            _mockTokenService
                .Setup(t => t.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            var result = await _authService.RegisterAsync(registerDto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
            result.ExpiresAt.Should().BeCloseTo(DateTime.UtcNow.AddHours(2), TimeSpan.FromMinutes(1));

            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u =>
                u.Username == "testuser" &&
                u.Email == "test@example.com" &&
                u.Role == "User")), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingUsername_ShouldThrowException()
        {
            // Arrange
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existing@example.com",
                PasswordHash = "hash",
                Role = "User"
            };

            var registerDto = new RegisterDto
            {
                Username = "existinguser",
                Email = "newemail@example.com",
                Password = "Test123!",
                Role = "User"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User> { existingUser });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(registerDto));

            exception.Message.Should().Be("Username já existe");
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task RegisterAsync_WithExistingEmail_ShouldThrowException()
        {
            // Arrange
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "existinguser",
                Email = "existing@example.com",
                PasswordHash = "hash",
                Role = "User"
            };

            var registerDto = new RegisterDto
            {
                Username = "newuser",
                Email = "existing@example.com",
                Password = "Test123!",
                Role = "User"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User> { existingUser });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _authService.RegisterAsync(registerDto));

            exception.Message.Should().Be("Email já cadastrado");
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var password = "Test123!";
            var passwordHash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    Encoding.UTF8.GetBytes(password)));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = passwordHash,
                Role = "User"
            };

            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = password
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User> { user });

            _mockTokenService
                .Setup(t => t.GenerateToken(It.IsAny<User>()))
                .Returns("fake-jwt-token");

            // Act
            var result = await _authService.LoginAsync(loginDto);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("fake-jwt-token");
            result.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");

            _mockTokenService.Verify(t => t.GenerateToken(It.Is<User>(u =>
                u.Username == "testuser")), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidUsername_ShouldThrowException()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "nonexistent",
                Password = "Test123!"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User>());

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(loginDto));

            exception.Message.Should().Be("Credenciais inválidas");
            _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_WithInvalidPassword_ShouldThrowException()
        {
            // Arrange
            var correctPassword = "Test123!";
            var passwordHash = Convert.ToBase64String(
                System.Security.Cryptography.SHA256.HashData(
                    System.Text.Encoding.UTF8.GetBytes(correctPassword)));

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = passwordHash,
                Role = "User"
            };

            var loginDto = new LoginDto
            {
                Username = "testuser",
                Password = "WrongPassword!"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User> { user });

            // Act & Assert
            var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => _authService.LoginAsync(loginDto));

            exception.Message.Should().Be("Credenciais inválidas");
            _mockTokenService.Verify(t => t.GenerateToken(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserExists_ShouldReturnUser()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "hash",
                Role = "User"
            };

            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User> { user });

            // Act
            var result = await _authService.GetUserByUsernameAsync("testuser");

            // Assert
            result.Should().NotBeNull();
            result!.Username.Should().Be("testuser");
            result.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task GetUserByUsernameAsync_WhenUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            _mockUserRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<User>());

            // Act
            var result = await _authService.GetUserByUsernameAsync("nonexistent");

            // Assert
            result.Should().BeNull();
        }
    }
}
