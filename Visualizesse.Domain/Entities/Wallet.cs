namespace Visualizesse.Domain.Entities;

public class Wallet(Guid id, Guid userId, string description = "")
{
    public Guid Id { get; private set; } = id;
    public decimal Balance { get; set; }
    public string? Description { get; set; } = description;
    public Guid UserId { get; private set; } = userId;
    public User User { get; set; }
}