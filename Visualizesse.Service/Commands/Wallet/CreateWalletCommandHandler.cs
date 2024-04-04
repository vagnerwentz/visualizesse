using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.Wallet;

public class CreateWalletCommandHandler(
    IWalletRepository walletRepository
    ) : IRequestHandler<CreateWalletCommand, OperationResult>
{
    public async Task<OperationResult> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var wallet = new Domain.Entities.Wallet(Guid.NewGuid(), request.UserId, request.Description);
            await walletRepository.Create(wallet, cancellationToken);

            return OperationResult.SuccessResult("Wallet created successfully", HttpStatusCode.Created);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
        
    }
}