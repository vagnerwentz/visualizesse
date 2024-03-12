using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Visualizesse.API;

public class CustomHeaderSwaggerAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (
            Relatives.paths.Contains(context.ApiDescription.RelativePath)
            )
        {
            operation.Parameters ??= new List<OpenApiParameter>();
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "x-user-identification",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "User identification header"
            });
        }
    }
}

public static class Relatives
{
    public static string[] paths =
    {
        "api/v1/transactions/mine",
        "api/v1/transactions/create",
    };
}