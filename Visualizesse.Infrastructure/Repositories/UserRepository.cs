using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class UserRepository(DatabaseContext databaseContext) : IUserRepository
{
    public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        await databaseContext.User.AddAsync(user, cancellationToken);
        await databaseContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<User?> FindByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await databaseContext.User.AsNoTracking().SingleOrDefaultAsync(u => u.Uuid == id, cancellationToken);
    }

    public async Task<bool> FindEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = await databaseContext.User.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    
        return user is not null;
    }

    public async Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken)
    {
        return await databaseContext.User.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
    }
}
