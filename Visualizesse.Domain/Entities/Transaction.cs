using Visualizesse.Domain.Enum;

namespace Visualizesse.Domain.Entities;

public class Transaction(
    Guid id, 
    Guid userId, 
    decimal value, 
    string? description, 
    string transactionType,
    int categoryId,
    int? subcategoryId,
    DateTime createdAt
    )
{
    public Guid Id { get; private set; } = id;
    public Guid UserId { get; private set; } = userId;
    
    public int CategoryId { get; private set; } = categoryId;
    public int? SubcategoryId { get; private set; } = subcategoryId;
    public Guid? WalletId { get; set; }
    public decimal Value { get; private set; } = value;
    public string? Description { get; private set; } = description;
    public string TransactionType { get; private set; } = transactionType;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public DateTime ChangedAt { get; private set; } = DateTime.Now;
    public Category Category { get; set; }
    public Subcategory Subcategory { get; set; }
    public Wallet Wallet { get; set; }

    public void EditTransaction(decimal? value, string? description, string? transactionType)
    {
        bool flag = false;

        if (!string.IsNullOrEmpty(description))
        {
            flag = true;
            Description = description;
        }


        if (value > 0)
        {
            flag = true;
            Value = value.Value;
        }


        if (!string.IsNullOrEmpty(transactionType))
        {
            flag = true;
            TransactionType = transactionType;
        }
            
        
        if (flag) ChangedAt = DateTime.Now;
    }
}