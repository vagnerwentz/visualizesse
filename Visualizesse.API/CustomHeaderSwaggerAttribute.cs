using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Visualizesse.API;

public class CustomHeaderSwaggerAttribute : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (!Relatives.paths.Contains(context.ApiDescription.RelativePath)) return;
        
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

public static class Relatives
{
    private const string Prefix = "api/v1";

    public static string[] paths =
    {
        $"{Prefix}/transactions/mine",
        $"{Prefix}/transactions/create",
        $"{Prefix}/subcategories/create_mine",
        $"{Prefix}/wallet/create"
    };
}