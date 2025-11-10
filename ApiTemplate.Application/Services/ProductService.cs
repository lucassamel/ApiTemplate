using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;

namespace ApiTemplate.Application.Services
{
    public class ProductService(IRepository<Product> repository) : IProductService
    {
        private readonly IRepository<Product> _repository = repository;

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);
            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(MapToDto);
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.AddAsync(product);
            return MapToDto(created);
        }

        private ProductDto MapToDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description
        };

        public Task UpdateProductAsync(Guid id, CreateProductDto dto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
