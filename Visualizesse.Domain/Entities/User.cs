using System.ComponentModel.DataAnnotations;

namespace Visualizesse.Domain.Entities;

public class User(Guid uuid, string name, string email, string password)
{
    public Guid Uuid { get; private set; } = uuid;
    public string Name { get; private set; } = name;
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public List<Transaction> Transactions { get; private set; } = new();
    public List<Wallet> Wallet { get; private set; } = new();
}