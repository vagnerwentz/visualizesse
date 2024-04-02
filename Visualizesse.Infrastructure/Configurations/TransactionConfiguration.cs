using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Infrastructure.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);
        
        
        builder.Property(t => t.Value);
        builder.Property(t => t.ChangedAt);
        builder.Property(t => t.CreatedAt);
        builder.Property(t => t.Description);
        builder.Property(t => t.TransactionType);
        builder.HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .IsRequired();
        
        builder.HasOne(t => t.Subcategory)
            .WithMany()
            .HasForeignKey(t => t.SubcategoryId);
        
        builder.HasOne(t => t.Wallet)
            .WithMany()
            .HasForeignKey(t => t.WalletId);
    }
}