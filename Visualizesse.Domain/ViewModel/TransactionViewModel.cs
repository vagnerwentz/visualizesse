namespace Visualizesse.Domain.ViewModel;

public class TransactionViewModel(
    Guid id, 
    string? description, 
    DateTime createdAt, 
    decimal value,
    string transactionType,
    string categoryDescription,
    string? subcategoryDescription,
    string? icon
    )
{
    public Guid Id { get; private set; } = id;
    public decimal Value { get; private set; } = value;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public string? Description { get; private set; } = description;
    public string TransactionType { get; private set; } = transactionType;
    public string CategoryDescription { get; private set; } = categoryDescription;
    public string? SubcategoryDescription { get; private set; } = subcategoryDescription;
    public string? Icon { get; private set; } = icon;
}