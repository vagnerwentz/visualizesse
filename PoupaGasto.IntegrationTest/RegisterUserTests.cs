// using PoupaGasto.IntegrationTest.Factory;
//
// using Visualizesse.Domain.Entities;
// using Visualizesse.Service.Commands.User;
//
// namespace PoupaGasto.IntegrationTest;
//
// public class RegisterUserTests : BaseIntegrationTest
// {
//     public RegisterUserTests(IntegrationTestWebAppFactory factory)
//         : base(factory)
//     {
//     }
//
//     [Fact]
//     public async Task Register_ShouldCreateUser()
//     {
//         // Arrange
//         var userCommand = new SignInCommand("John Doe", "johndoe@poupagasto.com.br", "johndoepoupagasto");
//         
//         // Act
//         var result = await Sender.Send(userCommand);
//         
//         var userData = (User)result.Data;
//         var user = DbContext.User.FirstOrDefault(u => u.Email == userData.Email);
//         
//         // Assert
//         Assert.NotNull(result);
//         Assert.True(result.Success);
//         Assert.StrictEqual(userData.Uuid, user!.Uuid);
//     }
// }