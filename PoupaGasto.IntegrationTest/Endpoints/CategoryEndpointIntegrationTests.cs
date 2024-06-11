using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Visualizesse.Domain.Entities;
using Visualizesse.Infrastructure;

namespace PoupaGasto.IntegrationTest.Endpoints;

[Collection("Category Integration Tests")]
public class CategoryEndpointIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public CategoryEndpointIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        CleanTable();
        InitializeDatabaseForTests();
    }

    [Theory(DisplayName = "GET all_categories endpoint should return status 200 with complete and accurate category data.")]
    [InlineData("api/v1/categories/all_categories")]
    public async Task GetAllCategories_EndpointReturnsCompleteAndAccurateCategoryData(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);
        
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        // Assert.Equal("text/html; charset=utf-8", 
        //     response.Content.Headers.ContentType.ToString());
        
        var responseData = await response.Content.ReadAsStringAsync();
        var categories = JsonConvert.DeserializeObject<List<Category>>(responseData);
        
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count);
        Assert.Contains(categories, c => c is { Description: "Tecnologia", Id: 1 });
        Assert.Contains(categories, c => c is { Description: "Educação", Id: 2 });
        Assert.Contains(categories, c => c is { Description: "Lazer", Id: 3 });
    }
    
    private void InitializeDatabaseForTests()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            db.Category.AddRange(
                new Category(1, "Tecnologia"),
                new Category (2, "Educação"),
                new Category( 3,"Lazer")
            );
            db.SaveChanges();
        }
    }

    private void CleanTable()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Category.RemoveRange(db.Category.ToList());
                db.SaveChanges();
                
                transaction.Commit();
            }
        }
    }
}