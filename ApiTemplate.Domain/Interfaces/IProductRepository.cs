using ApiTemplate.Domain.Entities;

namespace ApiTemplate.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task UpdateProduct(Guid id, Product product);
    }
}
