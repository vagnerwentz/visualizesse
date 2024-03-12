using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Uuid);

        // builder.Property(u => u.Name);
        // builder.Property(u => u.Email);
        // builder.Property(u => u.Password);
        // builder.Property(u => u.CreatedAt);
        builder
            .HasMany(u => u.Transactions)
            .WithOne()
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}