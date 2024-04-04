using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class CategoryRepository(DatabaseContext databaseContext): ICategoryRepository
{
    public async Task<bool> FindCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        var category = await databaseContext.Category.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
        
        if (category is null) return false;

        return true;
    }
}