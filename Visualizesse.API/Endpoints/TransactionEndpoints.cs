using MediatR;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Visualizesse.Service.Query.Transaction;
using Visualizesse.Service.Commands.Transaction;

namespace Visualizesse.API.Endpoints;

public static class TransactionEndpoints
{
    public static void RegisterTransactionEndpoints(this IEndpointRouteBuilder routes) 
    {
        var transactions = routes.MapGroup("api/v1/transactions");

        transactions.MapPost("create", async (
            HttpContext httpContext,
            TransactionService transactionService,
            TransactionCommand data
        ) => await transactionService.CreateTransactionAsync(data, httpContext))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Você poderá criar uma transação para o seu usuário.",
                Description = "Você deve usar no header `x-user-identification` que é o identificador do seu usuário e passar os campos necessários de uma transação."
            });;;
        
        transactions.MapGet("mine", async (
            HttpContext httpContext,
            TransactionService transactionService
        ) => await transactionService.GetMineTransactionsAsync(httpContext))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Você obterá todas as suas transações.",
                Description = "Você deve usar no header `x-user-identification` que é o identificador do seu usuário."
            });;
        
        transactions.MapPut("edit", async (
            TransactionService transactionService,
            EditTransactionCommand data
        ) => await transactionService.EditTransactionsByIdAsync(data))
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Você pode editar uma transação.",
                Description = "Para editar uma transação, você deve passar o `id` para buscar ela e pode passar os outros campos como" +
                              "`description`, `value`, `transactionType`, lembrando que estes valores devem ser passados apenas caso queira alterar os mesmos."
            });
        
        transactions.MapDelete("delete", async (
            TransactionService transactionService,
            [FromBody] DeleteTransactionCommand data
        ) => await transactionService.DeleteTransactionByIdAsync(data))
            .RequireAuthorization()
            .WithOpenApi(operation => new(operation)
        {
            Summary = "Você pode deletar uma transação..",
            Description = "No body você deve passar o id ta transação que você gostaria de deletar"
        });
    }
}

public class TransactionService(ILogger<TransactionService> logger, IMediator mediator)
{
    public async Task<IResult> CreateTransactionAsync(TransactionCommand data, HttpContext httpContext)
    {
        var userIdAsString = httpContext.Request.Headers["x-user-identification"].ToString();
        
        if (string.IsNullOrWhiteSpace(userIdAsString)) return TypedResults.BadRequest();
        
        data.UserId = Guid.Parse(userIdAsString);
        
        var result = await mediator.Send(data);
        
        if (result.Success == false)
        {
            logger.LogInformation(result.Message);
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok();
    }

    public async Task<IResult> GetMineTransactionsAsync(HttpContext httpContext)
    {
        #region Think to improve

        var userIdAsString = httpContext.Request.Headers["x-user-identification"].ToString();

        if (string.IsNullOrWhiteSpace(userIdAsString)) return TypedResults.BadRequest();

        #endregion
        
        var result = await mediator.Send(new GetMineTransactionQuery(Guid.Parse(userIdAsString)));

        return TypedResults.Ok(result);
    }

    public async Task<IResult> EditTransactionsByIdAsync(EditTransactionCommand data)
    {
        var result = await mediator.Send(data);

        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }
    
    public async Task<IResult> DeleteTransactionByIdAsync(DeleteTransactionCommand data)
    {
        var result = await mediator.Send(data);
        
        if (result.StatusCode == HttpStatusCode.InternalServerError)
        {
            return TypedResults.UnprocessableEntity(result);
        }
        
        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            return TypedResults.BadRequest(result);
        }
        
        return TypedResults.Ok(result);
    }
}