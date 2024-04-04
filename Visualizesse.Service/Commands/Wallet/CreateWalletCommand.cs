using MediatR;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.Wallet;

public class CreateWalletCommand(Guid userId, string description) : IRequest<OperationResult>
{
    public Guid UserId { get; private set; } = userId;
    public string Description { get; private set; } = description;
}