using System.Net;
using MediatR;
using Visualizesse.Domain.Event;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;
using Visualizesse.Infrastructure;

namespace Visualizesse.Service.Commands.Transaction;

public class TransactionCommandHandler(
        IUserRepository userRepository,
        ICategoryRepository categoryRepository,
        ITransactionRepository transactionRepository,
        IPublisher publisher,
        DatabaseContext databaseContext
    )
    : IRequestHandler<TransactionCommand, OperationResult>
{
    public async Task<OperationResult> Handle(TransactionCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await databaseContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (request.Value <= 0)
                return OperationResult.FailureResult(
                    "You can not add a transaction value with value less than equal zero.", HttpStatusCode.BadRequest);

            var categoryExists = await categoryRepository.FindCategoryByIdAsync(request.CategoryId, cancellationToken);

            if (!categoryExists)
                return OperationResult.FailureResult("We could not find the category.", HttpStatusCode.NotFound);

            var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

            if (user is null)
                return OperationResult.FailureResult("You cannot create a transaction for a non-existent user.",
                    HttpStatusCode.BadRequest);

            Domain.Entities.Transaction transactionToBeCreated = new Domain.Entities.Transaction(
                Guid.NewGuid(),
                request.UserId,
                request.Value,
                request.Description,
                request.TransactionType.ToString(),
                request.CategoryId,
                request.SubcategoryId,
                request.CreatedAt
            );
            transactionToBeCreated.WalletId = request.WalletId;

            await transactionRepository.CreateAsync(transactionToBeCreated, cancellationToken);
            var updateWalletBalanceEventRequest = new UpdateWalletBalanceEvent(
                request.UserId,
                request.WalletId,
                request.Value,
                request.TransactionType
            );

            await publisher.Publish(updateWalletBalanceEventRequest, cancellationToken);

            await databaseContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // Notificar para adicionar/retirar do balanço geral.

            return OperationResult.SuccessResult(HttpStatusCode.Created);
        }
        catch (UnauthorizedException exception)
        {
            return OperationResult.FailureResult(exception.Message, exception.StatusCode);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
    }
}