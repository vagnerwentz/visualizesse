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
    }
}