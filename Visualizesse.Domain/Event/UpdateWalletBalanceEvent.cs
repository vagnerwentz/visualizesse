using MediatR;
using Visualizesse.Domain.Enum;

namespace Visualizesse.Domain.Event;

public class UpdateWalletBalanceEvent(Guid userId, Guid walletId, decimal value, ETransaction transactionType) : INotification
{
    public Guid UserId { get; private set; } = userId;
    public Guid WalletId { get; private set; } = walletId;
    public decimal Amount { get; private set; } = value;
    public ETransaction TransactionType { get; private set; } = transactionType;
}