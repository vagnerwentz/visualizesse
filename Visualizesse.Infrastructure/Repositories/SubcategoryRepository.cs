using Microsoft.EntityFrameworkCore;
using Visualizesse.Domain.Entities;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Infrastructure.Repositories;

public class SubcategoryRepository(DatabaseContext context) : ISubcategoryRepository
{
    public async Task CreateMySubcategoryAsync(Subcategory subcategory, CancellationToken cancellationToken)
    {
        await context.Subcategory.AddAsync(subcategory, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}