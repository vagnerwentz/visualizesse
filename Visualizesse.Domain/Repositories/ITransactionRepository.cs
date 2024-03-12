using Visualizesse.Domain.Entities;

namespace Visualizesse.Domain.Repositories;

public interface ITransactionRepository
{
    Task UpdateTransactionAsync();
    Task DeleteTransactionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Transaction transaction, CancellationToken cancellationToken);
    Task<Transaction?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<Transaction>> GetTransactionsAsync(Guid id, CancellationToken cancellationToken);
}