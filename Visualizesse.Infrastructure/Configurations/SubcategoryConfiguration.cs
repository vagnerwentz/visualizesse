using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Infrastructure.Configurations;

public class SubcategoryConfiguration : IEntityTypeConfiguration<Subcategory>
{
    public void Configure(EntityTypeBuilder<Subcategory> builder)
    {
        builder.HasKey(s => s.Id);
        builder.Property(s => s.Description).IsRequired().HasMaxLength(100);

        // Relacionamento com Category
        builder.HasOne(s => s.Category)
            .WithMany()
            .HasForeignKey(s => s.CategoryId)
            .IsRequired();

        // Relacionamento com User
        builder.HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .IsRequired();
    }
}