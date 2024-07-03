using System.Net;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Enum;
using Visualizesse.Domain.Event;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Infrastructure;

namespace Visualizesse.Service.EventHandler;

public class UpdateWalletBalanceEventHandler(DatabaseContext databaseContext) : INotificationHandler<UpdateWalletBalanceEvent>
{
    public async Task Handle(UpdateWalletBalanceEvent notification, CancellationToken cancellationToken)
    {
        var wallet = await databaseContext.Wallet
            .SingleOrDefaultAsync(w => w.UserId == notification.UserId && w.Id == notification.WalletId, cancellationToken);

        if (wallet is null || wallet.UserId != notification.UserId)
            throw new UnauthorizedException("Wallet not found or unauthorized access.", HttpStatusCode.Unauthorized);
        
        switch (notification.Event)
        {
            case "DELETION" when notification.TransactionType == ETransaction.Income.ToString():
                wallet!.Balance -= notification.Amount;
                break;
            case "DELETION":
                wallet!.Balance += notification.Amount;
                break;
            default:
            {
                if (notification.TransactionType == ETransaction.Income.ToString())
                {
                    wallet!.Balance += notification.Amount;
                }
                else
                {
                    wallet!.Balance -= notification.Amount;
                }

                break;
            }
        }
    }
}