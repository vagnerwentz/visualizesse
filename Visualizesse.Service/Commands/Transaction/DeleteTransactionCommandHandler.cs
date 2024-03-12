using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.Transaction;

public class DeleteTransactionCommandHandler(ITransactionRepository transactionRepository) : IRequestHandler<DeleteTransactionCommand, OperationResult>
{
    
    public async Task<OperationResult> Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await transactionRepository.DeleteTransactionByIdAsync(request.Id, cancellationToken);
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