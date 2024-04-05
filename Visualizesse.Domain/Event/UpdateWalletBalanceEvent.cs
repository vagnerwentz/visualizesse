using MediatR;
using Visualizesse.Domain.Enum;

namespace Visualizesse.Domain.Event;

public class UpdateWalletBalanceEvent(
    Guid userId, 
    Guid walletId, 
    decimal value, 
    string transactionType, 
    string? @event) : INotification
{
    public Guid UserId { get; private set; } = userId;
    public Guid WalletId { get; private set; } = walletId;
    public decimal Amount { get; private set; } = value;
    public string TransactionType { get; private set; } = transactionType;
    public string? Event { get; private set; } = @event;
}