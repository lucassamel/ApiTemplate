using ApiTemplate.Application.DTOs;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ApiTemplate.IntegrationTests.Controllers
{
    public class AuthControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public AuthControllerIntegrationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        [Fact]
        public async Task Register_WithValidData_ShouldReturnOkAndToken()
        {
            // Arrange
            var registerDto = new RegisterDto
            {
                Username = $"testuser_{Guid.NewGuid():N}",
                Email = $"test_{Guid.NewGuid():N}@example.com",
                Password = "Test123!",
                Role = "User"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>(_jsonOptions);
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrEmpty();
            result.Username.Should().Be(registerDto.Username);
            result.Email.Should().Be(registerDto.Email);
            result.ExpiresAt.Should().BeAfter(DateTime.UtcNow);
        }

        [Fact]
        public async Task Register_WithDuplicateUsername_ShouldReturnBadRequest()
        {
            // Arrange
            var username = $"duplicate_{Guid.NewGuid():N}";
            var firstRegister = new RegisterDto
            {
                Username = username,
                Email = $"first_{Guid.NewGuid():N}@example.com",
                Password = "Test123!",
                Role = "User"
            };

            var secondRegister = new RegisterDto
            {
                Username = username,
                Email = $"second_{Guid.NewGuid():N}@example.com",
                Password = "Test123!",
                Role = "User"
            };

            // Act
            await _client.PostAsJsonAsync("/api/auth/register", firstRegister);
            var response = await _client.PostAsJsonAsync("/api/auth/register", secondRegister);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Username já existe");
        }

        [Fact]
        public async Task Register_WithDuplicateEmail_ShouldReturnBadRequest()
        {
            // Arrange
            var email = $"duplicate_{Guid.NewGuid():N}@example.com";
            var firstRegister = new RegisterDto
            {
                Username = $"user1_{Guid.NewGuid():N}",
                Email = email,
                Password = "Test123!",
                Role = "User"
            };

            var secondRegister = new RegisterDto
            {
                Username = $"user2_{Guid.NewGuid():N}",
                Email = email,
                Password = "Test123!",
                Role = "User"
            };

            // Act
            await _client.PostAsJsonAsync("/api/auth/register", firstRegister);
            var response = await _client.PostAsJsonAsync("/api/auth/register", secondRegister);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Email já cadastrado");
        }

        [Fact]
        public async Task Login_WithValidCredentials_ShouldReturnOkAndToken()
        {
            // Arrange
            var username = $"logintest_{Guid.NewGuid():N}";
            var password = "Test123!";

            var registerDto = new RegisterDto
            {
                Username = username,
                Email = $"{username}@example.com",
                Password = password,
                Role = "User"
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var result = await response.Content.ReadFromJsonAsync<AuthResponseDto>(_jsonOptions);
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrEmpty();
            result.Username.Should().Be(username);
        }

        [Fact]
        public async Task Login_WithInvalidUsername_ShouldReturnUnauthorized()
        {
            // Arrange
            var loginDto = new LoginDto
            {
                Username = "nonexistentuser",
                Password = "Test123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Credenciais inválidas");
        }

        [Fact]
        public async Task Login_WithInvalidPassword_ShouldReturnUnauthorized()
        {
            // Arrange
            var username = $"passwordtest_{Guid.NewGuid():N}";
            var registerDto = new RegisterDto
            {
                Username = username,
                Email = $"{username}@example.com",
                Password = "CorrectPassword123!",
                Role = "User"
            };

            await _client.PostAsJsonAsync("/api/auth/register", registerDto);

            var loginDto = new LoginDto
            {
                Username = username,
                Password = "WrongPassword123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/auth/login", loginDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Credenciais inválidas");
        }

        [Fact]
        public async Task RegisterAndLogin_FullFlow_ShouldWork()
        {
            // Arrange
            var username = $"fullflow_{Guid.NewGuid():N}";
            var password = "FullFlow123!";
            var email = $"{username}@example.com";

            var registerDto = new RegisterDto
            {
                Username = username,
                Email = email,
                Password = password,
                Role = "Admin"
            };

            // Act - Register
            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", registerDto);
            var registerResult = await registerResponse.Content.ReadFromJsonAsync<AuthResponseDto>(_jsonOptions);

            // Act - Login
            var loginDto = new LoginDto
            {
                Username = username,
                Password = password
            };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginResult = await loginResponse.Content.ReadFromJsonAsync<AuthResponseDto>(_jsonOptions);

            // Assert
            registerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            registerResult.Should().NotBeNull();
            registerResult!.Username.Should().Be(username);

            loginResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            loginResult.Should().NotBeNull();
            loginResult!.Username.Should().Be(username);
            loginResult.Token.Should().NotBeNullOrEmpty();
        }
    }
}
