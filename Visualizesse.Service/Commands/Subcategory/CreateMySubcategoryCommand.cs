using MediatR;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.Subcategory;

public class CreateMySubcategoryCommand(Guid userId, int categoryId, string description) : IRequest<OperationResult>
{
    public Guid UserId { get; set; } = userId;
    public int CategoryId { get; set; } = categoryId;
    public string Description { get; set; } = description;
}