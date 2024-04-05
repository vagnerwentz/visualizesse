namespace Visualizesse.Domain.Repositories;

public interface ICategoryRepository
{
    Task<bool> FindCategoryByIdAsync(int id, CancellationToken cancellationToken);
}