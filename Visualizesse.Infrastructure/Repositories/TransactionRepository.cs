using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class TransactionRepository
    //(
    //List<Transaction> transactions
    // DatabaseContext databaseContext
    //) 
    : ITransactionRepository
{
    private readonly List<Transaction> _transactions;
    public TransactionRepository()
    {
        _transactions = new();
    }
    public async Task DeleteTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var transactionToBeDeleted = _transactions.Find(t => t.Id == id);

        if (transactionToBeDeleted != null)
        {
            _transactions.Remove(transactionToBeDeleted);    
        }
        else
        {
            throw new Exception($"Transaction with identifier {id} could not be found.");
        }
        
        // var transaction = await databaseContext.Transaction.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
        //
        // if (transaction != null) 
        // {
        //     
        //     databaseContext.Transaction.Remove(transaction);
        //     await databaseContext.SaveChangesAsync(cancellationToken);
        // }
        // else
        // {
        //     throw new Exception($"Transaction with identifier {id} could not be found.");
        // }
        
    }

    public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken)
    {
        _transactions.Add(transaction);
        
        // await databaseContext.Transaction.AddAsync(transaction, cancellationToken);
        // await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Transaction?> GetTransactionByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _transactions.Find(t => t.Id == id);
        
        // return await databaseContext.Transaction.
        //     SingleOrDefaultAsync(t => t.Id == id, cancellationToken); 
    }

    public async Task<List<Transaction>> GetTransactionsAsync(Guid id, CancellationToken cancellationToken)
    {
        return _transactions.Where(t => t.UserId == id).ToList();
        
        // return databaseContext.Transaction.Where(t => t.UserId == id).AsNoTracking().ToList();
    }

    public async Task UpdateTransactionAsync()
    {
        // await databaseContext.SaveChangesAsync();
    }
}