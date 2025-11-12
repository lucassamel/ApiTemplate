using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTemplate.Infrastructure.Mappings
{
    public class ProductMapper : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
             .ToTable("Products")
             .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier");

            builder
                .Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .Property( x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME")
                .HasDefaultValue(DateTime.UtcNow);

            builder
                .Property(x => x.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("NVARCHAR(200)");

            builder
                .Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("NVARCHAR(1000)");

            builder
                .Property(x => x.Price)
                .IsRequired()
                .HasColumnName("Price")
                .HasColumnType("DECIMAL(18,2)");
        }   
    
    }
}
