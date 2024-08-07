using Visualizesse.Infrastructure;

namespace Visualizesse.API.Endpoints;

public static class CategoryEndpoints
{
    public static void RegisterCategoriesEndpoints(this IEndpointRouteBuilder routes) 
    {
        var categories = routes.MapGroup("api/v1/categories").RequireCors();

        categories.MapGet("all_categories", async (
            HttpContext httpContext,
            DatabaseContext context
        ) => context.Category.ToList());

    }
}