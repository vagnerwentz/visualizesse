using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Visualizesse.Domain.Entities;

namespace Visualizesse.Infrastructure;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {

    }

    public DbSet<User> User { get; set; }
    public DbSet<Transaction> Transaction { get; set; }
    public DbSet<Category> Category { get; set; }
    public DbSet<Subcategory> Subcategory { get; set; }
    public DbSet<Wallet> Wallet { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100); // Limita o tamanho máximo para 100 caracteres

            // Adicionando uma Check Constraint para garantir que o nome não seja uma string vazia
            entity.HasCheckConstraint("CK_User_Name_NotEmpty", "\"Name\" <> ''");
        });
    }
}
