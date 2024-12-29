using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;

namespace Visualizesse.Service.Commands.User;


public record Reponse(string Token, string Name, string Email);
public class SignInCommandHandler(
    IAuthService authService,
    IUserRepository userRepository
) : IRequestHandler<SignInCommand, OperationResult>
{
    public async Task<OperationResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FindUserByEmailAsync(request.Email, cancellationToken);

        if (user is null) 
            return OperationResult.FailureResult("Please, verify if you are typing the e-mail and password correctly.", HttpStatusCode.BadRequest);

        var isEqual = authService.CompareComputedSHA256Hash(request.Password, user.Password);

        if (!isEqual) return OperationResult.FailureResult("Please, verify if you are typing the e-mail and password correctly.", HttpStatusCode.BadRequest);
        
        var token = authService.GenerateJWTToken(user);

        var data = new Reponse(token, user.Name, user.Email);
        
        return OperationResult.SuccessResult(data, HttpStatusCode.Accepted);
    }
}