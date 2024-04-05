// using MediatR;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using PoupaGasto.IntegrationTest.Factory;
// using Visualizesse.Infrastructure;
//
// namespace PoupaGasto.IntegrationTest;
//
// public abstract class BaseIntegrationTest
//     : IClassFixture<WebApplicationFactory<Program>>,
//         IDisposable
// {
//     private readonly IServiceScope _scope;
//     protected readonly ISender Sender;
//     protected readonly DatabaseContext DbContext;
//
//     protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
//     {
//         _scope = factory.Services.CreateScope();
//
//         Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
//
//         DbContext = _scope.ServiceProvider
//             .GetRequiredService<DatabaseContext>();
//     }
//
//     public void Dispose()
//     {
//         _scope?.Dispose();
//         DbContext?.Dispose();
//     }
// }