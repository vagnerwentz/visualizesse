using MediatR;
using Visualizesse.Domain.ViewModel;

namespace Visualizesse.Service.Query.Transaction;

public class GetMineTransactionQuery(Guid userId) : IRequest<List<TransactionViewModel>>
{
    public Guid UserId { get; private set; } = userId;
}