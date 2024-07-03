using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PoupaGasto.IntegrationTest.Helpers;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Enum;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Infrastructure;
using Visualizesse.Service.Commands.Transaction;

namespace PoupaGasto.IntegrationTest.Endpoints;

[Collection("Integration Tests")]
public class TransactionEndpointIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private const string Password = "Password123@_";
    private const string Email = "johndoe@poupagasto.com.br";
    private const string CreateTransactionUrl = "api/v1/transactions/create";
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TransactionEndpointIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        CleanTable();
    }

    [Fact(DisplayName =
        "POST /api/v1/transactions/create for an is null or white space x-user-identification header value should return HTTP 400 Bad Request.")]
    public async Task Post_Transactions_Create_ForIsNullOrWhiteSpaceXUserIdentificationHeader_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);

        await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", string.Empty);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var command = new Fixture().Create<TransactionCommand>();

        // Act
        var response = await client.PostAsJsonAsync(CreateTransactionUrl, command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact(DisplayName =
        "POST /api/v1/transactions/create for an invalid transaction value should return HTTP 400 Bad Request.")]
    public async Task Post_Transactions_Create_ForInvalidTransactionValue_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);
        var walletHelper = new WalletHelper(client);

        var userId = await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);
        await walletHelper.CreateWalletAsync("Banco BTG Pactual", token, userId);
        var wallet = GetWalletByUserId(userId);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", userId);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var command = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, -1)
            .With(t => t.Description, "Transação de R$ -1,00")
            .With(t => t.TransactionType, ETransaction.Income)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, 1)
            .With(t => t.WalletId, wallet?.Id)
            .Create();

        // {
        //     "value": 300,
        //     "description": "Transação de R$ 300,00",
        //     "transactionType": 1, // 0 income 1 outcome
        //     "createdAt": "2024-04-05T01:13:34.532Z",
        //     "categoryId": 4,
        //     "walletId": "d9b8624c-4dd4-4b2e-9ec3-5052d3027cf9"
        // }


        // Act
        var response = await client.PostAsJsonAsync(CreateTransactionUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal("You can not add a transaction value with value less than equal zero.", result.Message);
    }

    [Fact(DisplayName =
        "POST /api/v1/transactions/create for an no-existent category should return HTTP 400 Bad Request.")]
    public async Task Post_Transactions_Create_ForANoExistentCategory_ShouldReturnNotFound()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);
        var walletHelper = new WalletHelper(client);

        var userId = await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);
        await walletHelper.CreateWalletAsync("Banco BTG Pactual", token, userId);
        var wallet = GetWalletByUserId(userId);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", userId);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var command = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, 10)
            .With(t => t.Description, "Transação de R$ 10,00")
            .With(t => t.TransactionType, ETransaction.Income)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, -1)
            .With(t => t.WalletId, wallet?.Id)
            .Create();

        // Act
        var response = await client.PostAsJsonAsync(CreateTransactionUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        Assert.Equal("We could not find the category.", result.Message);
    }

    [Fact(DisplayName =
        "POST /api/v1/transactions/create for an no-existent category should return HTTP 400 Bad Request.")]
    public async Task Post_Transactions_Create_ForANoExistentUser_ShouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);
        var walletHelper = new WalletHelper(client);

        var userId = await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);
        await walletHelper.CreateWalletAsync("Banco BTG Pactual", token, userId);
        var wallet = GetWalletByUserId(userId);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", "41edcbb8-6047-42b3-bf03-3ed19a98c1c0");
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var command = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, 10)
            .With(t => t.Description, "Transação de R$ 10,00")
            .With(t => t.TransactionType, ETransaction.Income)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, 1)
            .With(t => t.WalletId, wallet?.Id)
            .Create();

        // Act
        var response = await client.PostAsJsonAsync(CreateTransactionUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("You cannot create a transaction for a non-existent user.", result.Message);
    }

    [Fact(DisplayName =
        "POST /api/v1/transactions/create should return HTTP 200 OK and update wallet balance for a valid income transaction.")]
    public async Task Post_Transactions_Create_IncomeType_ShouldReturnOkAndUpdateWalletBalance()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);
        var walletHelper = new WalletHelper(client);

        var userId = await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);
        await walletHelper.CreateWalletAsync("Banco BTG Pactual", token, userId);
        var wallet = GetWalletByUserId(userId);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", userId);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var command = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, 10)
            .With(t => t.Description, "Transação de R$ 10,00")
            .With(t => t.TransactionType, ETransaction.Income)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, 1)
            .With(t => t.WalletId, wallet?.Id)
            .Without(t => t.SubcategoryId)
            .Create();

        // Act
        var response = await client.PostAsJsonAsync(CreateTransactionUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
        var walletAfterTransactionIncome = GetWalletByUserId(userId);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotNull(walletAfterTransactionIncome);
        Assert.Equal(command.Value, walletAfterTransactionIncome.Balance);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
    }

    [Fact(DisplayName = "POST /api/v1/transactions/create should return HTTP 200 OK and update wallet balance correctly for income and outcome transactions.")]
    public async Task Post_Transactions_Create_IncomeAndOutcome_ShouldReturnOkAndUpdateWalletBalanceCorrectly()
    {
        // Arrange
        var client = _factory.CreateClient();
        var userHelper = new UserHelper(client);
        var walletHelper = new WalletHelper(client);

        var userId = await userHelper.CreateUserAsync("John Doe", Email, Password);
        var token = await userHelper.AuthenticateUserAsync(Email, Password);
        await walletHelper.CreateWalletAsync("Banco BTG Pactual", token, userId);
        var wallet = GetWalletByUserId(userId);

        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("x-user-identification", userId);
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var incomeCommand = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, (decimal)125.50)
            .With(t => t.Description, "Transação de R$ 125,50")
            .With(t => t.TransactionType, ETransaction.Income)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, 1)
            .With(t => t.WalletId, wallet?.Id)
            .Without(t => t.SubcategoryId)
            .Create();
        
        var outcomeCommand = new Fixture()
            .Build<TransactionCommand>()
            .With(t => t.Value, (decimal)100.25)
            .With(t => t.Description, "Transação de R$ 100,25")
            .With(t => t.TransactionType, ETransaction.Outcome)
            .With(t => t.CreatedAt, DateTime.Now)
            .With(t => t.CategoryId, 1)
            .With(t => t.WalletId, wallet?.Id)
            .Without(t => t.SubcategoryId)
            .Create();

        // Act
        var incomeResponse = await client.PostAsJsonAsync(CreateTransactionUrl, incomeCommand);
        var outcomeResponse = await client.PostAsJsonAsync(CreateTransactionUrl, outcomeCommand);
        var incomeResponseData = await incomeResponse.Content.ReadAsStringAsync();
        var outcomeResponseData = await outcomeResponse.Content.ReadAsStringAsync();
        var incomeResult = JsonConvert.DeserializeObject<OperationResult>(incomeResponseData);
        var outcomeResult = JsonConvert.DeserializeObject<OperationResult>(outcomeResponseData);
        
        var walletAfterTransactionIncome = GetWalletByUserId(userId);
        var value = incomeCommand.Value - outcomeCommand.Value;

        // Assert
        Assert.Equal(HttpStatusCode.OK, incomeResponse.StatusCode);
        Assert.Equal(HttpStatusCode.OK, outcomeResponse.StatusCode);
        Assert.NotNull(incomeResult);
        Assert.NotNull(outcomeResult);
        Assert.NotNull(walletAfterTransactionIncome);
        Assert.Equal(value, walletAfterTransactionIncome.Balance);
        Assert.Equal(HttpStatusCode.Created, incomeResult.StatusCode);
        Assert.Equal(HttpStatusCode.Created, outcomeResult.StatusCode);
    }

private void CleanTable()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.Transaction.RemoveRange(db.Transaction.ToList());
                db.Wallet.RemoveRange(db.Wallet.ToList());
                db.User.RemoveRange(db.User.ToList());
                db.SaveChanges();
                
                transaction.Commit();
            }
        }
    }

    private Wallet? GetWalletByUserId(string userId)
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            return db.Wallet.SingleOrDefault(w => w.UserId.ToString() == userId);
        }
    }

    public void Dispose()
    {
        CleanTable();
    }
}