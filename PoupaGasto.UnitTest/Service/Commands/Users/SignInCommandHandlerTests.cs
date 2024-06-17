
using System.Net;
using Moq;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.Services;
using Visualizesse.Service.Commands.User;

namespace PoupaGasto.UnitTest.Service.Commands.Users;

public class SignInCommandHandlerTests
{
    private readonly Mock<IAuthService> _mockAuthService = new Mock<IAuthService>();
    private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
    private readonly SignInCommandHandler _handler;

    public SignInCommandHandlerTests()
    {
        _handler = new SignInCommandHandler(_mockAuthService.Object, _mockUserRepository.Object);
    }
    
    [Fact(DisplayName = "[SignInCommandHandlerTests] Should return a BadRequest status code and a message if user is not found.")]
    public async Task Handle_UserNotFound_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new SignInCommand("johndoe@poupagasto.com","password123");
        
        _mockUserRepository.Setup(x => x.FindUserByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as User);

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Please, verify if you are typing the e-mail and password correctly.", result.Message);
    }
    
    [Fact(DisplayName = "[SignInCommandHandlerTests] Should return a BadRequest status code and a message if password is incorrect.")]
    public async Task Handle_PasswordMismatch_ShouldReturnBadRequest()
    {
        // Arrange
        var command = new SignInCommand("johndoe@poupagasto.com","password123");
        var user = new User(Guid.NewGuid(), "John Doe", "johndoe@poupagasto.com", "hashedPassword123");
        
        _mockUserRepository.Setup(x => x.FindUserByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockAuthService.Setup(x => x.CompareComputedSHA256Hash(command.Password, user.Password)).Returns(false);

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.False(result.Success);
        Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        Assert.Equal("Please, verify if you are typing the e-mail and password correctly.", result.Message);
    }
    
    [Fact(DisplayName = "[SignInCommandHandlerTests] Should return an Accepted status code and a token if e-mail and password are valid.")]
    public async Task Handle_ValidCredentials_ShouldReturnAcceptedAndToken()
    {
        // Arrange
        var command = new SignInCommand("johndoe@poupagasto.com","password123");
        var user = new User(Guid.NewGuid(), "John Doe", "johndoe@poupagasto.com", "hashedPassword123");
        
        _mockUserRepository.Setup(x => x.FindUserByEmailAsync(command.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockAuthService.Setup(x => x.CompareComputedSHA256Hash(command.Password, user.Password)).Returns(true);
        
        _mockAuthService.Setup(x => x.GenerateJWTToken(user)).Returns("mocked_token_value");

        // Act
        var result = await _handler.Handle(command, new CancellationToken());

        // Assert
        Assert.True(result.Success);
        Assert.NotNull(result.Data);
        Assert.Equal(HttpStatusCode.Accepted, result.StatusCode);
    }
}