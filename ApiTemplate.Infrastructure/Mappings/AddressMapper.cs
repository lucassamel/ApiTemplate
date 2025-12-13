using ApiTemplate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiTemplate.Infrastructure.Mappings
{
    public class AddressMapper : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder
             .ToTable("Addresses")
             .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("uniqueidentifier"); ;

            builder
                .Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt")
                .HasColumnType("DATETIME")
                .HasDefaultValueSql("GETUTCDATE()");

            builder
                .Property(x => x.UpdatedAt)
                .IsRequired(false)
                .HasColumnName("UpdatedAt")
                .HasColumnType("DATETIME");

            builder
                .Property(x => x.Cep)
                .IsRequired()
                .HasColumnName("Cep")
                .HasColumnType("VARCHAR")
                .HasMaxLength(8);

            builder
                .Property(x => x.Uf)
                .IsRequired()
                .HasColumnName("Uf")
                .HasColumnType("VARCHAR")
                .HasMaxLength(2);

            builder
                .HasOne(x => x.User)
                .WithOne(x => x.Address)
                .HasForeignKey<Address>(x => x.UserId);
        }
    }
}
