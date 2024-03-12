using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Visualizesse.Service.Commands.User;

namespace Visualizesse.API.Endpoints;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes) 
    {
        var users = routes.MapGroup("api/v1/users");

        users.MapPost("register", async (
            UserService userService,
            SignInCommand data
        ) => await userService.CreateUserAsync(data));
    }
}

public class UserService(ILogger<UserService> logger, IMediator mediator)
{
    public async Task<IResult> CreateUserAsync(SignInCommand data)
    {
        var result = await mediator.Send(data);
        
        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }
}