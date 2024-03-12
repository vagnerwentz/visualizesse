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
