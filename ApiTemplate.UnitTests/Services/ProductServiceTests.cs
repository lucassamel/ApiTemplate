using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Services;
using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;
using FluentAssertions;
using Moq;

namespace ApiTemplate.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IRepository<Product>> _mockRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockRepository = new Mock<IRepository<Product>>();
            _productService = new ProductService(_mockRepository.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_WhenProductExists_ShouldReturnProductDto()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product
            {
                Id = productId,
                Name = "Test Product",
                Price = 99.99m,
                Description = "Test Description",
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(productId);
            result.Name.Should().Be("Test Product");
            result.Price.Should().Be(99.99m);
            result.Description.Should().Be("Test Description");

            _mockRepository.Verify(r => r.GetByIdAsync(productId), Times.Once);
        }
        [Fact]
        public async Task GetProductByIdAsync_WhenProductDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var productId = Guid.NewGuid();

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act
            //var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.GetProductByIdAsync(productId));
            
            _mockRepository.Verify(r => r.GetByIdAsync(productId), Times.Once);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 10m, Description = "Desc 1" },
            new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 20m, Description = "Desc 2" },
            new() { Id = Guid.NewGuid(), Name = "Product 3", Price = 30m, Description = "Desc 3" }
        };

            _mockRepository
                .Setup(r => r.GetAllAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            result.Should().HaveCount(3);
            result.Should().OnlyContain(p => p.Name.StartsWith("Product"));
            _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateProductAsync_WithValidData_ShouldCreateAndReturnProduct()
        {
            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 49.99m,
                Description = "New Description"
            };

            var createdProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Price = createDto.Price,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // Act
            var result = await _productService.CreateProductAsync(createDto);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be("New Product");
            result.Price.Should().Be(49.99m);
            result.Description.Should().Be("New Description");

            _mockRepository.Verify(r => r.AddAsync(It.Is<Product>(p =>
                p.Name == createDto.Name &&
                p.Price == createDto.Price)), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductExists_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Old Name",
                Price = 10m,
                Description = "Old Description",
                CreatedAt = DateTime.UtcNow
            };

            var updateDto = new CreateProductDto
            {
                Name = "Updated Name",
                Price = 20m,
                Description = "Updated Description"
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _mockRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.UpdateProductAsync(productId, updateDto);

            // Assert
            _mockRepository.Verify(r => r.GetByIdAsync(productId), Times.Once);
            _mockRepository.Verify(r => r.UpdateAsync(It.Is<Product>(p =>
                p.Id == productId &&
                p.Name == "Updated Name" &&
                p.Price == 20m)), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updateDto = new CreateProductDto
            {
                Name = "Updated Name",
                Price = 20m,
                Description = "Updated Description"
            };

            _mockRepository
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync((Product?)null);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _productService.UpdateProductAsync(productId, updateDto));

            _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Product>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldCallRepositoryDelete()
        {
            // Arrange           
            var createDto = new CreateProductDto
            {
                Name = "New Product",
                Price = 49.99m,
                Description = "New Description"
            };

            var createdProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Price = createDto.Price,
                Description = createDto.Description,
                CreatedAt = DateTime.UtcNow
            };

            _mockRepository
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(createdProduct);

            // ensure GetByIdAsync returns the product so service won't throw
            _mockRepository
                .Setup(r => r.GetByIdAsync(createdProduct.Id))
                .ReturnsAsync((Product?)createdProduct);

            _mockRepository
                .Setup(r => r.DeleteAsync(createdProduct.Id))
                .Returns(Task.CompletedTask);

            // Act
            await _productService.DeleteProductAsync(createdProduct.Id);

            // Assert
            _mockRepository.Verify(r => r.DeleteAsync(createdProduct.Id), Times.Once);
        }
    }
}
