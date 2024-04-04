using Visualizesse.Domain.Entities;

namespace Visualizesse.Domain.Repositories;

public interface ISubcategoryRepository
{
    Task CreateMySubcategoryAsync(Subcategory subcategory, CancellationToken cancellationToken);
}