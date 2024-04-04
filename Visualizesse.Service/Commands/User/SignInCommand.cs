using MediatR;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.User;

public class SignInCommand(string email, string password) : IRequest<OperationResult>
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
}