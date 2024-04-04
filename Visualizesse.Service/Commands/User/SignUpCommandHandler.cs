using System.Net;
using MediatR;
using Npgsql;
using Visualizesse.Domain.Exceptions;
using UserEntity = Visualizesse.Domain.Entities.User;
using Visualizesse.Domain.Helpers;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;

namespace Visualizesse.Service.Commands.User;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, OperationResult>
{
    private readonly IAuthService _authService;
    private readonly IUserRepository _userRepository;
    
    public SignUpCommandHandler(
        IAuthService authService,
        IUserRepository userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public async Task<OperationResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var emailAlreadyExists = await _userRepository.FindEmailAsync(request.Email,cancellationToken);

            if (emailAlreadyExists) return OperationResult.FailureResult("E-mail is already in use.", HttpStatusCode.Conflict);
            
            var user = new UserEntity(Guid.NewGuid(), request.Name, request.Email, _authService.ComputeSHA256Hash(request.Password));

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