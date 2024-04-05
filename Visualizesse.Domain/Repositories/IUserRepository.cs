using Visualizesse.Domain.Entities;

namespace Visualizesse.Domain.Repositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user, CancellationToken cancellationToken);
    Task<User?> FindByIdAsync(Guid id,CancellationToken cancellationToken);
    Task<bool> FindEmailAsync(string email, CancellationToken cancellationToken);
    Task<User?> FindUserByEmailAsync(string email, CancellationToken cancellationToken);
}
