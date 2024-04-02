using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class TransactionRepository(DatabaseContext databaseContext) : ITransactionRepository
{
    // private readonly List<Transaction> _transactions;
    // public TransactionRepository()
    // {
    //     _transactions = new();
    //     var transaction = new Transaction(
    //         Guid.Parse("c1b1ef5e-44c2-4b37-8310-bec4767cfbc3"), 
    //         Guid.Parse("5049b9a6-c45c-4054-9c24-c5bcaa43cf17"), 
    //         10, 
    //         "Açaí", 
    //         "Outcome");
    //     _transactions.Add(transaction);
    // }
    public async Task DeleteTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transaction = await databaseContext.Transaction.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        
        if (transaction != null) 
        {
            
            databaseContext.Transaction.Remove(transaction);
            await databaseContext.SaveChangesAsync(cancellationToken);
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
        return await databaseContext.Transaction.
            SingleOrDefaultAsync(t => t.Id == id, cancellationToken); 
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