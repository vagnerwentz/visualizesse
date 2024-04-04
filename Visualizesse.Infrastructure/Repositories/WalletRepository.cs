using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class WalletRepository(DatabaseContext databaseContext) : IWalletRepository
{
    public async Task<Wallet?> GetWalletByUserIdAsync(Guid walletId, CancellationToken cancellationToken)
    {
        return await databaseContext.Wallet.SingleOrDefaultAsync(w => w.Id == walletId, cancellationToken);
    }

    public async Task SaveChangesAsync()
    {
        await databaseContext.SaveChangesAsync();
    }

    public async Task Create(Wallet wallet, CancellationToken cancellationToken)
    {
        
        await databaseContext.Wallet.AddAsync(wallet, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }
}