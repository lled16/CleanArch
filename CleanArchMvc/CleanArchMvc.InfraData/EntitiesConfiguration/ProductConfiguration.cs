using CleanArchMvc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.InfraData.EntitiesConfiguration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // Conigurações utilizadas para configurar a FluentAPI,
            // pois senão o Entity Framework geraria a tabela com varchar de valores máximos e nullables
            // e decimais com muitas casas

           builder.HasKey(x => x.Id);

           builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
           builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
           builder.Property(x => x.Price).HasPrecision(10,2);

           builder.HasOne(e => e.Category).WithMany(e => e.Products).HasForeignKey(e => e.CategoryId);
           // WithMany e HasforeignKey utilizados para fixar que será um relacionamento de um para muitos

        }
    }
}
