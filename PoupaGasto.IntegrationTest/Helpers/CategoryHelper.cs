using Microsoft.Extensions.DependencyInjection;
using Visualizesse.Domain.Entities;
using Visualizesse.Infrastructure;

namespace PoupaGasto.IntegrationTest.Helpers;

public class CategoryHelper(CustomWebApplicationFactory<Program> factory)
{
    public async Task InitializeSeed()
    {
        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        db.Category.AddRange(
            new Category(1, "Assinaturas"),
            new Category (2, "Educação"),
            new Category( 3,"Lazer")
        );
        await db.SaveChangesAsync();
    }
}