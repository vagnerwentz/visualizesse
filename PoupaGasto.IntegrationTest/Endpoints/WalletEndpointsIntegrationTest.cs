using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PoupaGasto.IntegrationTest.Helpers;
using Visualizesse.API.Request.Wallet;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Infrastructure;

namespace PoupaGasto.IntegrationTest.Endpoints;

[Collection("Integration Tests")]
public class WalletEndpointsIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private const string CreateWalletUrl = "api/v1/wallet/create";
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public WalletEndpointsIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        CleanTable();
    }

    [Fact(DisplayName = "POST /api/v1/wallet/create for a valid user should return HTTP 200 OK and create a wallet.")]
    public async Task CreateWallet_ForValidUser_ReturnsOkAndCreatesWallet()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);

        var userId = await userHelper.CreateUserAsync("Vagner Wentz", "vagnerwentz@poupagasto.com.br", "Password123@_");
        var token = await userHelper.AuthenticateUserAsync("vagnerwentz@poupagasto.com.br", "Password123@_");

        var walletCommand = new CreateWalletRequest("Banco BTG Pactual");
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Add("x-user-identification", userId);
        
        // Act
        var response = await client.PostAsJsonAsync(CreateWalletUrl, walletCommand);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.Equal("Wallet created successfully", result.Message);
    }
    
    private void CleanTable()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Wallet.RemoveRange(db.Wallet.ToList());
                db.User.RemoveRange(db.User.ToList());
                db.SaveChanges();
                
                transaction.Commit();
            }
        }
    }

    public void Dispose()
    {
        CleanTable();
    }
}