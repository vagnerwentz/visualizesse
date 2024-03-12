namespace Visualizesse.Domain.ViewModel;

public class TransactionViewModel(Guid id, string? description, DateTime createdAt, decimal value)
{
    public Guid Id { get; private set; } = id;
    public decimal Value { get; private set; } = value;
    public DateTime CreatedAt { get; private set; } = createdAt;
    public string? Description { get; private set; } = description;
}