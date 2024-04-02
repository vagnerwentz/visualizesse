using MediatR;
using Visualizesse.Domain.Repositories;
using Visualizesse.Domain.ViewModel;

namespace Visualizesse.Service.Query.Transaction;

public class GetMineTransactionQueryHandler(ITransactionRepository transactionRepository)
    : IRequestHandler<GetMineTransactionQuery, List<TransactionViewModel>>
{
    public async Task<List<TransactionViewModel>> Handle(GetMineTransactionQuery request, CancellationToken cancellationToken)
    {
        var data = transactionRepository.GetTransactionsAsync(request.UserId, cancellationToken);

        var transactionViewModel = data
            .Select(t => 
                new TransactionViewModel(
                    t.Id, 
                    t.Description, 
                    t.CreatedAt, 
                    t.Value,
                    t.TransactionType,
                    t.Category.Description,
                    t.Subcategory?.Description))
            .ToList();

        return transactionViewModel;
    }
}