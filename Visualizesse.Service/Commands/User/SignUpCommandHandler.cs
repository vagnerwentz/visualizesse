using System.Net;
using MediatR;
using Npgsql;
using Visualizesse.Domain.Exceptions;
using UserEntity = Visualizesse.Domain.Entities.User;
using Visualizesse.Domain.Helpers;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;

namespace Visualizesse.Service.Commands.User;

public class SignUpCommandHandler(
    IAuthService authService,
    IUserRepository userRepository)
    : IRequestHandler<SignUpCommand, OperationResult>
{
    public async Task<OperationResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailAlreadyExists = await userRepository.FindEmailAsync(request.Email,cancellationToken);

            if (emailAlreadyExists) return OperationResult.FailureResult("E-mail is already in use.", HttpStatusCode.Conflict);
            
            var user = new UserEntity(Guid.NewGuid(), string.IsNullOrEmpty(request.Name) ? null : request.Name, request.Email, authService.ComputeSHA256Hash(request.Password));

            await userRepository.CreateUserAsync(user, cancellationToken);

            return OperationResult.SuccessResult(user, HttpStatusCode.Created);
        }
        catch (InvalidOperationException exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.BadRequest);
        }
        catch (Exception exception)
        {
            return OperationResult.ExceptionResult(exception.ToString(), exception.Message, HttpStatusCode.InternalServerError);
        }
    }
}