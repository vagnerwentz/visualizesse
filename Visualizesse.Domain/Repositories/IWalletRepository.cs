using Visualizesse.Domain.Entities;

namespace Visualizesse.Domain.Repositories;

public interface IWalletRepository
{
    Task<Wallet?> GetWalletByUserIdAsync(Guid walletId, CancellationToken cancellationToken);

    Task SaveChangesAsync();
    Task Create(Wallet wallet, CancellationToken cancellationToken);
}