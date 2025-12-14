using ApiTemplate.Application.DTOs;
using ApiTemplate.Application.Interfaces;
using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;

namespace ApiTemplate.Application.Services
{
    public class ProductService(IRepository<Product> repository) : IProductService
    {
        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await repository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Produto não encontrado.");

            return MapToDto(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await repository.GetAllAsync();
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

            var created = await repository.AddAsync(product);
            return MapToDto(created);
        }

        private ProductDto MapToDto(Product product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description,
            Count = product.Count
        };

        public async Task UpdateProductAsync(Guid id, CreateProductDto dto)
        {
            var existingProduct = await repository.GetByIdAsync(id) ?? 
                throw new InvalidOperationException("Produto não encontrado.");
            
            existingProduct.Name = dto.Name;
            existingProduct.Price = dto.Price;
            existingProduct.Description = dto.Description;
            existingProduct.UpdatedAt = DateTime.UtcNow;
            existingProduct.Count = dto.Count;

            await repository.UpdateAsync(existingProduct);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var existingProduct = await repository.GetByIdAsync(id) ??
                throw new InvalidOperationException("Produto não encontrado.");

            await repository.DeleteAsync(existingProduct.Id);
        }
    }
}
