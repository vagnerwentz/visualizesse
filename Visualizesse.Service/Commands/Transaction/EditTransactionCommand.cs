using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using Visualizesse.Domain.Enum;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.Transaction;

public class EditTransactionCommand : IRequest<OperationResult>
{
    public EditTransactionCommand(Guid id, string description, decimal? value, ETransaction? transactionType)
    {
        Id = id;
        Description = description;
        Value = value;
        TransactionType = transactionType;
    }
    
    [Required]
    public Guid Id { get; private set; }

    public string Description { get; private set; }
    public decimal? Value { get; private set; }
    public ETransaction? TransactionType { get; private set; }
}