using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.Transaction;

public class TransactionCommandHandler(IUserRepository userRepository, ITransactionRepository transactionRepository)
    : IRequestHandler<TransactionCommand, OperationResult>
{
    public async Task<OperationResult> Handle(TransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Value <= 0)
                return OperationResult.FailureResult(
                    "You can not add a transaction value with value less than equal zero.", HttpStatusCode.BadRequest);
            
            var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);

            if (user is null) return OperationResult.FailureResult("You cannot create a transaction for a non-existent user.", HttpStatusCode.BadRequest);
        
            Domain.Entities.Transaction transaction = new Domain.Entities.Transaction(
                Guid.NewGuid(),
                request.UserId,
                request.Value,
                request.Description,
                request.TransactionType.ToString()
            );

            await transactionRepository.CreateAsync(transaction, cancellationToken);
        
            // Notificar para adicionar/retirar do balanço geral.

            return OperationResult.SuccessResult(HttpStatusCode.Created);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
    }
}