using System.Net;
using Moq;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;
using Visualizesse.Service.Commands.User;

namespace PoupaGasto.UnitTest.Service.Commands.Users;

public class SignUpCommandHandlerTests
{
    private readonly SignUpCommandHandler _handler;
    private readonly Mock<IAuthService> _mockAuthService = new Mock<IAuthService>();
    private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    
    public SignUpCommandHandlerTests()
    {
        _handler = new SignUpCommandHandler(_mockAuthService.Object, _mockUserRepository.Object);
    }
    
    [Fact(DisplayName = "[SignUpCommandHandlerTests] Should return a Success result and Created status code when a new user is successfully created.")]
    public async Task Handle_WhenUserIsSuccessfullyCreated_ReturnsSuccessResult()
    {
        // Arrange
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "password123");
        var expectedUser = new User(Guid.NewGuid(), "John Doe", "johndoe@poupagasto.com.br", "ComputedSHA256Hash");

        _mockUserRepository
            .Setup(x => x.FindEmailAsync(command.Email,It.IsAny<CancellationToken>())
            ).ReturnsAsync(false);

        _mockAuthService.Setup(x => x.ComputeSHA256Hash(command.Password)).Returns("ComputedSHA256Hash");
        
        _mockUserRepository
            .Setup(x => x.CreateUserAsync(expectedUser, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.Success);
        Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        
        var user = Assert.IsType<User>(result.Data);
        Assert.Equal(expectedUser.Name, user.Name);
        Assert.Equal(expectedUser.Email, user.Email);
        Assert.Equal(expectedUser.Password, user.Password);
    }

    [Fact(DisplayName = "[SignUpCommandHandlerTests] Should return a Conflict status and message when e-mail already exists.")]
    public async Task Handle_WhenEmailAlreadyExists_ReturnsConflictStatusCode()
    {
        // Arrange
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "password123");

        _mockUserRepository
            .Setup(x => x.FindEmailAsync(command.Email,It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.NotEmpty(result.Message);
        Assert.Equal(HttpStatusCode.Conflict, result.StatusCode);
    }
    
    [Fact(DisplayName = "[SignUpCommandHandlerTests] Should return a BadRequest result when an InvalidOperationException is thrown in Handle method.")]
    public async Task Handle_WhenInvalidOperationExceptionThrown_ReturnsAppropriateStatus()
    {
        // Arrange
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "password123");

        _mockUserRepository
            .Setup(x => x.FindEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Simulated InvalidOperationException."));

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Contains("Simulated InvalidOperationException", result.Message);
    }
    
    [Fact(DisplayName = "[SignUpCommandHandlerTests] Should return an Internal Server Error result when an exception is thrown in Handle method.")]
    public async Task Handle_WhenExceptionThrown_ReturnsInternalServerErrorStatusCode()
    {
        // Arrange
        var command = new SignUpCommand("John Doe", "johndoe@poupagasto.com.br", "password123");

        _mockUserRepository
            .Setup(
                x => x.FindEmailAsync(
                    command.Email,
                    It.IsAny<CancellationToken>()
                )
            ).ThrowsAsync(new Exception("FindEmailAsync threw an exception."));

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.NotEmpty(result.Message);
        Assert.NotEmpty(result.Exception);
        Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
    }
}