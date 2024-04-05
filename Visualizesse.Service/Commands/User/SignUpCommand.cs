using MediatR;
using System.ComponentModel.DataAnnotations;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.User;

public record SignUpCommand(
    [Required] string Name, 
    [Required] string Email, 
    [Required] string Password
) : IRequest<OperationResult>;