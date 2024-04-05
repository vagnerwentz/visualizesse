using System.Net;
using MediatR;
using Visualizesse.Domain.Exceptions;
using Visualizesse.Domain.Repositories;

namespace Visualizesse.Service.Commands.Subcategory;

public class CreateMySubcategoryCommandHandler(
    IUserRepository userRepository,
    ICategoryRepository categoryRepository,
    ISubcategoryRepository subcategoryRepository
    ) : IRequestHandler<CreateMySubcategoryCommand, OperationResult>
{
    
    public async Task<OperationResult> Handle(CreateMySubcategoryCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Description)) 
            return OperationResult.FailureResult("You should have a description to create your subcategory.", HttpStatusCode.BadRequest);
        
        var user = await userRepository.FindByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return OperationResult.FailureResult("We had a problem to find the user.", HttpStatusCode.NotFound);

        var categoryExists = await categoryRepository.FindCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (!categoryExists) return OperationResult.FailureResult("We can not find the category.", HttpStatusCode.NotFound);

        var subcategory = new Visualizesse.Domain.Entities.Subcategory
            (request.Description, request.CategoryId, request.UserId); 
        
        await subcategoryRepository.CreateMySubcategoryAsync(subcategory, cancellationToken);
        
        return OperationResult.SuccessResult(HttpStatusCode.Created);
    }
}