// using MediatR;
// using System.Net;
// using Microsoft.AspNetCore.Mvc;
// using Visualizesse.Service.Query.Transaction;
// using Visualizesse.Service.Commands.Transaction;
//
// namespace Visualizesse.API.Controllers;
//
// [ApiController]
// [Route("api/transaction")]
// public class TransactionController : ControllerBase
// {
//     private readonly IMediator _mediator;
//     private readonly ILogger<UserController> _logger;
//
//     public TransactionController(
//         IMediator mediator,
//         ILogger<UserController> logger
//     )
//     {
//         _mediator = mediator;
//         _logger = logger;
//     }
//
//     [HttpPost("create")]
//     public async Task<IActionResult> CreateTransaction([FromBody] TransactionCommand data)
//     {
//         #region Think to improve
//
//         var userIdAsString = Request.Headers["x-user-identification"].ToString();
//
//         if (string.IsNullOrWhiteSpace(userIdAsString)) return BadRequest();
//         
//         data.UserId = Guid.Parse(userIdAsString);
//
//         #endregion
//         
//         var result = await _mediator.Send(data);
//         
//         if (result.Success == false)
//         {
//             _logger.LogInformation(result.Message);
//             return BadRequest(result);
//         }
//         
//         return Ok(result);
//     }
//
//     [HttpGet("mine")]
//     public async Task<IActionResult> GetMineTransactions()
//     {
//         #region Think to improve
//
//         var userIdAsString = Request.Headers["x-user-identification"].ToString();
//
//         if (string.IsNullOrWhiteSpace(userIdAsString)) return BadRequest();
//
//         #endregion
//         
//         var data = await _mediator.Send(new GetMineTransactionQuery(Guid.Parse(userIdAsString)));
//
//         return Ok(data);
//     }
//     
//     [HttpPut("edit")]
//     public async Task<IActionResult> EditTransactionByIdAsync([FromBody] EditTransactionCommand data)
//     {
//         var result = await _mediator.Send(data);
//
//         if (result.StatusCode == HttpStatusCode.BadRequest)
//         {
//             return BadRequest(result);
//         }
//         
//         return Ok(result);
//     }
//     
//     [HttpDelete("delete")]
//     public async Task<IActionResult> DeleteTransactionById([FromBody] DeleteTransactionCommand data)
//     {
//         var result = await _mediator.Send(data);
//         
//         if (result.StatusCode == HttpStatusCode.InternalServerError)
//         {
//             return UnprocessableEntity(result);
//         }
//         
//         if (result.StatusCode == HttpStatusCode.BadRequest)
//         {
//             return BadRequest(result);
//         }
//         
//         return Ok(result);
//     }
// }