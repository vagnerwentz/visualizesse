using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class TransactionRepository(DatabaseContext databaseContext) : ITransactionRepository
{
    public async Task DeleteTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await databaseContext.Transaction.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (transaction != null) 
        {
            databaseContext.Transaction.Remove(transaction);
        }
        else
        {
            throw new Exception($"Transaction with identifier {id} could not be found.");
        }
    }

    public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        await databaseContext.Transaction.AddAsync(transaction, cancellationToken);
        // await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await databaseContext.Transaction
            .Include(t => t.Wallet)
            .SingleOrDefaultAsync(t => t.Id == id, cancellationToken); 
    }

    public List<Transaction> GetTransactionsAsync(Guid id, CancellationToken cancellationToken)
    {
        return databaseContext.Transaction.Where(t => t.UserId == id)
            .Include(t => t.Category)
            .Include(t => t.Subcategory)
            .AsNoTracking().ToList();
    }

    public async Task UpdateTransactionAsync()
    {
        await databaseContext.SaveChangesAsync();
    }
}