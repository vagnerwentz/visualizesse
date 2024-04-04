// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.AspNetCore.TestHost;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.Extensions.DependencyInjection;
// using Xunit;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using Testcontainers.PostgreSql;
// using Visualizesse.Infrastructure;
//
// namespace PoupaGasto.IntegrationTest.Factory;
//
// public class IntegrationTestWebAppFactory
//     : WebApplicationFactory<Program>,
//         IAsyncLifetime
// {
//     private readonly PostgreSqlContainer _postgreSqlContainer = new PostgreSqlBuilder()
//         .WithImage("postgres:14.7")
//         .WithDatabase("db")
//         .WithUsername("postgres")
//         .WithPassword("postgres")
//         .WithCleanUp(true)
//         .Build(); 
//     
//     protected override void ConfigureWebHost(IWebHostBuilder builder)
//     {
//         builder.ConfigureTestServices(services =>
//         {
//             var descriptorType =
//                 typeof(DbContextOptions<DatabaseContext>);
//
//             var descriptor = services
//                 .SingleOrDefault(s => s.ServiceType == descriptorType);
//
//             if (descriptor is not null)
//             {
//                 services.Remove(descriptor);
//             }
//
//             services.AddDbContext<DatabaseContext>(options =>
//                 options.UseNpgsql(_postgreSqlContainer.GetConnectionString()));
//         });
//     }
//
//     public Task InitializeAsync()
//     {
//         return _postgreSqlContainer.StartAsync();
//     }
//
//     public new Task DisposeAsync()
//     {
//         return _postgreSqlContainer.StopAsync();
//     }
// }