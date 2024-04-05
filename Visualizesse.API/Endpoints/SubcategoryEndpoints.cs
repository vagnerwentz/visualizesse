using MediatR;
using Visualizesse.API.Request.Subcategory;
using Visualizesse.Infrastructure;
using Visualizesse.Service.Commands.Subcategory;

namespace Visualizesse.API.Endpoints;

public static class SubcategoryEndpoints
{
    public static void RegisterSubcategoriesEndpoints(this IEndpointRouteBuilder routes) 
    {
        var subcategories = routes.MapGroup("api/v1/subcategories");

        subcategories.MapPost("create_mine", async (
            HttpContext httpContext,
            CreateMySubcategoryRequest data,
            SubcategoryService subcategoryService
        ) => await subcategoryService.CreateMySubcategoryAsync(data, httpContext)).WithOpenApi(operation => new(operation)
        {
            Summary = "Criar uma subcategoria para o seu usuário.",
            Description = "Para criar uma subcategoria, você deverá passar o header `x-user-identification`, o `categoryId` que você " +
                          "gostaria de relacionar, e também uma `description`, todos os campos são obrigatórios."
        });

    }
}

public class SubcategoryService(ILogger<SubcategoryService> logger, IMediator mediator)
{
    public async Task<IResult> CreateMySubcategoryAsync(CreateMySubcategoryRequest data, HttpContext httpContext)
    {
        var userIdAsString = IdentificadonHeader.GetHeader(httpContext);

        var subcategoryCommand = new CreateMySubcategoryCommand(Guid.Parse(userIdAsString), data.CategoryId, data.Description);
        
        var result = await mediator.Send(subcategoryCommand);

        return TypedResults.Ok(result);
    }
}