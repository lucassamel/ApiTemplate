using ApiTemplate.Application.DTOs;

namespace ApiTemplate.Application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> CreateProductAsync(CreateProductDto dto);
        Task UpdateProductAsync(Guid id, CreateProductDto dto);
        Task DeleteProductAsync(Guid id);
    }
}