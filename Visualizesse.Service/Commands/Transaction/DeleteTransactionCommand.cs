using MediatR;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.Transaction;

public class DeleteTransactionCommand(Guid id) : IRequest<OperationResult>
{
    public Guid Id { get; private set; } = id;
}