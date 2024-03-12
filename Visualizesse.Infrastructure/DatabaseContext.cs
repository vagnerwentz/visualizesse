using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Transaction> Transaction { get; set; }

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}