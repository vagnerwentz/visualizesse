using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.Transaction;

public class EditTransactionCommandHandler(ITransactionRepository transactionRepository) : IRequestHandler<EditTransactionCommand, OperationResult>
{
    
    public async Task<OperationResult> Handle(EditTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (request.Value <= 0) return OperationResult.FailureResult("You cannot edit a transaction by setting it to a value less than or equal to zero.", HttpStatusCode.BadRequest);

            var transaction = await transactionRepository.GetTransactionByIdAsync(request.Id, cancellationToken);

            if (transaction is null)
                return OperationResult.FailureResult($"We could not find a transaction with identifier {request.Id}", HttpStatusCode.NotFound);
        
            transaction.EditTransaction(request.Value, request.Description, request.TransactionType.ToString());

            await transactionRepository.UpdateTransactionAsync();
        
            return OperationResult.SuccessResult(HttpStatusCode.NoContent);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
        
    }
}