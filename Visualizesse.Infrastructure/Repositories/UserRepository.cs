using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class UserRepository/**(DatabaseContext databaseContext)**/ : IUserRepository
{
    private readonly List<User> _users;
    public UserRepository()
    {
        _users = new();
        var user = new User
            (Guid.Parse("5049b9a6-c45c-4054-9c24-c5bcaa43cf17"), "Carlos", "carlos@gmail.com", "5baa61e4c9b93f3f0682250b6cf8331b7ee68fd8");
        _users.Add(user);
    }
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
    { 
        _users.Add(user);
        // await databaseContext.User.AddAsync(user, cancellationToken);
        // await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _users.Find(u => u.Uuid == id);
        // return await databaseContext.User.AsNoTracking().SingleOrDefaultAsync(u => u.Uuid == id, cancellationToken);
    }

    public async Task<bool> FindEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = _users.Find(u => u.Email == email);
        
        // var user = await databaseContext.User.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    
        if (user is null) return false;
    
        return  true;
    }
}
