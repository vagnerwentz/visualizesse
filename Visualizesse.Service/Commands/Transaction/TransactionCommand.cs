using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MediatR;
using Visualizesse.Domain.Enum;
using Visualizesse.Domain.Exceptions;

namespace Visualizesse.Service.Commands.Transaction;

public class TransactionCommand : IRequest<OperationResult>
{
    [JsonIgnore]
    public Guid UserId { get; set; }
    
    [Required]
    public decimal Value { get; set; }
    
    public string? Description { get; set; }
    
    [Required]
    public ETransaction TransactionType { get; set; }
}