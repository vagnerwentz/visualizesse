using System.Net;
using MediatR;
using Npgsql;
using Visualizesse.Domain.Exceptions;
using UserEntity = Visualizesse.Domain.Entities.User;
using Visualizesse.Domain.Helpers;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.User;

public class SignInCommandHandler : IRequestHandler<SignInCommand, OperationResult>
{
    private readonly IUserRepository _userRepository;
    
    public SignInCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<OperationResult> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailAlreadyExists = await _userRepository.FindEmailAsync(request.Email,cancellationToken);

            if (emailAlreadyExists) return OperationResult.FailureResult("E-mail is already in use.", HttpStatusCode.Conflict);

            var encrypted = EncryptationManager.Encrypt(request.Password);
            
            var user = new UserEntity(Guid.NewGuid(), request.Name, request.Email, encrypted);

            await _userRepository.CreateUserAsync(user, cancellationToken);

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