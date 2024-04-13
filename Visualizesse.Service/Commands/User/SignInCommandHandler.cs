using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;

namespace Visualizesse.Service.Commands.User;

public class SignInCommandHandler(
    IAuthService authService,
    IUserRepository userRepository,
    ISessionRepository sessionRepository
) : IRequestHandler<SignInCommand, OperationResult>
{
    public async Task<OperationResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await userRepository.FindUserByEmailAsync(request.Email, cancellationToken);

            if (user is null) 
                return OperationResult.FailureResult("Please, verify if you are typing the e-mail and password correctly.", HttpStatusCode.BadRequest);

            var isEqual = authService.CompareComputedSHA256Hash(request.Password, user.Password);

            if (!isEqual) return OperationResult.FailureResult("Please, verify if you are typing the e-mail and password correctly.", HttpStatusCode.BadRequest);
        
            var token = authService.GenerateJWTToken(user);
            
            await sessionRepository.CreateSessionAsync(new Domain.Entities.Session(token, user.Uuid));

            return OperationResult.SuccessResult(token, HttpStatusCode.Accepted);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message,
                HttpStatusCode.InsufficientStorage);
        }
    }
}