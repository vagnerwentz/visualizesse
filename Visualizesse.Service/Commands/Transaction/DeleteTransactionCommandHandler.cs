using System.Net;
using MediatR;
using Visualizesse.Domain.Enum;
using Visualizesse.Domain.Event;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;
using Visualizesse.Infrastructure;

namespace Visualizesse.Service.Commands.Transaction;

public class DeleteTransactionCommandHandler(
    ITransactionRepository transactionRepository,
    IPublisher publisher,
    DatabaseContext databaseContext
    ) : IRequestHandler<DeleteTransactionCommand, OperationResult>
{
    
    public async Task<OperationResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var n = DateTime.UtcNow;
            var date = new DateOnly();
            using var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);
            var transactionToBeDeleted = await transactionRepository.GetTransactionByIdAsync(request.Id, cancellationToken);

            if (transactionToBeDeleted is null) return OperationResult.FailureResult("It was not able to find the transaction.", HttpStatusCode.NotFound);
            
            var updateWalletBalanceEventRequest = new UpdateWalletBalanceEvent(
                transactionToBeDeleted.UserId,
                transactionToBeDeleted.WalletId,
                transactionToBeDeleted.Value,
                transactionToBeDeleted.TransactionType,
                "DELETION"
            );
            
            await transactionRepository.DeleteTransactionByIdAsync(request.Id, cancellationToken);
            await publisher.Publish(updateWalletBalanceEventRequest, cancellationToken);
            
            await databaseContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            
            return OperationResult.SuccessResult(HttpStatusCode.NoContent);
        }
        catch (Exception exception)
        {
            if (exception.Message == $"Transaction with identifier {request.Id} could not be found.")
                return OperationResult.ExceptionResult(exception.Message, HttpStatusCode.BadRequest);
            
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
    }
}