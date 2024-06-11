using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Infrastructure;
using Visualizesse.Service.Commands.User;

namespace PoupaGasto.IntegrationTest.Endpoints;

[Collection("Integration Tests")]
public class UserEndpointsIntegrationTest
    : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private const string RegisterUrl = "/api/v1/users/register";
    private const string SignInUrl = "/api/v1/users/signin";
    private readonly CustomWebApplicationFactory<Program> _factory;
    
    public UserEndpointsIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        CleanTable();
    }
    
    [Fact(DisplayName = $"POST {RegisterUrl} should return success and create a new user if data is valid.")]
    public async Task PostRegisterUser_ReturnsSuccessAndCreatesUser()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "Password123@_");
        
        // Act
        var response = await client.PostAsJsonAsync(RegisterUrl, command);
        
        // Assert
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
        var user = JsonConvert.DeserializeObject<User>(result.Data.ToString());
        
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        Assert.NotNull(user);
        Assert.Equal(command.Name, user.Name);
        Assert.Equal(command.Email, user.Email);
        Assert.NotEqual(command.Password, user.Password);
    }
    
    [Fact(DisplayName = $"POST {RegisterUrl} should return HTTP 409 Conflict when the email is already in use and should not create a new user.")]
    public async Task PostRegisterUser_WithEmailAlreadyInUse_ReturnsConflictAndDoesNotCreateUser()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "Password123@_");
        var stringContent = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
        SetupInitialData();
        
        // Act
        var response = await client.PostAsync(RegisterUrl, stringContent);
        
        // Assert
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
        
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
        Assert.Equal("E-mail is already in use.", result.Message);
    }
    
    [Fact(DisplayName = $"POST {RegisterUrl} with invalid data should return HTTP 500 InternalServerError and indicate an invalid operation.")]
    public async Task PostRegisterUser_WithInvalidOperation_ReturnsInternalServerError()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignUpCommand("", "invalidemail", "Password123@_");
        
        // Act
        var response = await client.PostAsJsonAsync(RegisterUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
         
        // Assert
        Assert.NotNull(result);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        
        Assert.False(result.Success);
        Assert.NotEmpty(result.Message);
        Assert.NotEmpty(result.Exception);
    }

    [Fact(DisplayName = $"POST {SignInUrl} with valid credentials should return HTTP 202 Accepted and a JWT token.")]
    public async Task SignIn_WithValidCredentials_ReturnsAcceptedAndToken()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignInCommand("johndoe@poupagasto.com.br", "Password123@_");
        SetupInitialData();
        
        // Act
        var response = await client.PostAsJsonAsync(SignInUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
    }
    
    [Fact(DisplayName = $"POST {SignInUrl} with non-existent user should return HTTP 400 Bad Request and appropriate error message.")]
    public async Task SignIn_WithNonExistentUser_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignInCommand("johndoe@poupagasto.com.br", "Password123@_");
        
        // Act
        var response = await client.PostAsJsonAsync(SignInUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Please, verify if you are typing the e-mail and password correctly.", result.Message);
    }
    
    [Fact(DisplayName = $"POST {SignInUrl} with existent user but with wrong password should return HTTP 400 Bad Request and appropriate error message.")]
    public async Task SignIn_WithExistentUser_ReturnsBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new SignInCommand("johndoe@poupagasto.com.br", "WrongPassword123@_");
        SetupInitialData();
        
        // Act
        var response = await client.PostAsJsonAsync(SignInUrl, command);
        var responseData = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<OperationResult>(responseData);
    
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.NotNull(result);
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Please, verify if you are typing the e-mail and password correctly.", result.Message);
    }

    private void CleanTable()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.User.RemoveRange(db.User.ToList());
                db.SaveChanges();
                
                transaction.Commit();
            }
        }
    }
    
    private void SetupInitialData()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (var transaction = db.Database.BeginTransaction())
            {
                db.User.RemoveRange(db.User.ToList());
                db.SaveChanges();
                
                var user = new User(
                    Guid.NewGuid(),
                    "John Doe",
                    "johndoe@poupagasto.com.br",
                    "df62d30b371bc51cb495e6c216687419137a30c279462796d2b7bdd74b06a9d5"
                );
                db.User.Add(user);
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