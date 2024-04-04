using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Visualizesse.Infrastructure.Configurations;

public class WalletConfiguration: IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.HasKey(w => w.Id);
        
        builder.HasOne(w => w.User)
            .WithMany(u => u.Wallet)
            .HasForeignKey(w => w.UserId)  // Chave estrangeira para UserId
            .IsRequired();
    }
}