using MediatR;
using Visualizesse.Service.Commands.Session;
using Visualizesse.Service.Commands.User;

namespace Visualizesse.API.Endpoints;

public static class UserEndpoints
{
    public static void RegisterUserEndpoints(this IEndpointRouteBuilder routes) 
    {
        var users = routes.MapGroup("api/v1/users");

        users.MapPost("register", async (
            UserService userService,
            SignUpCommand data
        ) => await userService.CreateUserAsync(data));
        
        users.MapPost("signin", async (
            UserService userService,
            SignInCommand data
        ) => await userService.SignIn(data));
        
        users.MapDelete("logout", async (
            UserService userService,
            HttpContext httpContext
        ) => await userService.Logout(httpContext)).RequireAuthorization();
    }
}

public class UserService(ILogger<UserService> logger, IMediator mediator)
{
    public async Task<IResult> CreateUserAsync(SignUpCommand data)
    {
        var result = await mediator.Send(data);
        
        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }

    public async Task<IResult> SignIn(SignInCommand data)
    {
        var result = await mediator.Send(data);
        
        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }
    
    public async Task<IResult> Logout(HttpContext httpContext)
    {
        var token = httpContext.Request.Headers["Authorization"].ToString();
        var splitedToken = token.Split(" ");
        var result =
            await mediator.Send(new InvalidateCommand(splitedToken[1]));
        
        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }
}