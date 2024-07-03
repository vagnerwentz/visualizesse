using MediatR;
using Visualizesse.API.Request.Subcategory;
using Visualizesse.API.Request.Wallet;
using Visualizesse.Service.Commands.Subcategory;
using Visualizesse.Service.Commands.Wallet;

namespace Visualizesse.API.Endpoints;

public static class WalletEndpoints
{
    public static void RegisterWalletEndpoints(this IEndpointRouteBuilder routes) 
    {
        var wallet = routes.MapGroup("api/v1/wallet");

        wallet.MapPost("create", async (
            HttpContext httpContext,
            WalletService walletService,
            CreateWalletRequest data
        ) => await walletService.CreateWallet(httpContext, data))
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
        {
            Summary = "Criar uma carteira para o seu usuário.",
            Description = "Para criar uma wallet, você deverá passar o header `x-user-identification`."
        });

    }
}

public class WalletService(ILogger<SubcategoryService> logger, IMediator mediator)
{
    public async Task<IResult> CreateWallet(HttpContext httpContext, CreateWalletRequest data)
    {
        var userIdAsString = IdentificadonHeader.GetHeader(httpContext);
        var createWalledCommand = new CreateWalletCommand(Guid.Parse(userIdAsString), data.Description);
        var result = await mediator.Send(createWalledCommand);

        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }

        return TypedResults.Ok(result);
    }
}