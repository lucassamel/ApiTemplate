using ApiTemplate.Domain.Entities;
using ApiTemplate.Domain.Interfaces;
using ApiTemplate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ApiTemplate.Infrastructure.Repositories
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        public async Task UpdateProduct(Guid id, Product product)
        {
            var existingProduct = await context.Products
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync() ?? throw new Exception("Produto não encontrado");

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Count = product.Count;

            await context.SaveChangesAsync();
        }
    }
}
