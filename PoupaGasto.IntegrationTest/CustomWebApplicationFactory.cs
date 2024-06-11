using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Visualizesse.Domain.Entities;
using Visualizesse.Infrastructure;

namespace PoupaGasto.IntegrationTest;

public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remova a descrição do DbContext existente
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DatabaseContext>));
            services.Remove(descriptor);

            // Adicione o DbContext com a configuração padrão
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=poupagasto-integration-test;Database=poupagasto-integration-test");
            });
            
            // Crie um ServiceProvider temporário para aplicar migrations
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                db.Database.Migrate();  // Aplica as migrations
            }
            
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                if (db.Database.EnsureCreated())
                {
                    InitializeDatabaseForTests(db);
                }
            }
        });

        builder.UseEnvironment("Development");
    }

    private void InitializeDatabaseForTests(DatabaseContext database)
    {
        database.Category.AddRange(
            new Category(1, "Transporte"),
            new Category(2, "Alimentação")
        );
        database.SaveChanges();
    }
}