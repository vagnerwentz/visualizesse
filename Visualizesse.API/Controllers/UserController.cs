using MediatR;
using Microsoft.AspNetCore.Mvc;
using Visualizesse.Service.Commands.User;

namespace Visualizesse.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(
        IMediator mediator,
        ILogger<UserController> logger
    )
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand data) 
    {
        var result = await _mediator.Send(data);
        
        if (result.Success == false)
        {
            _logger.LogInformation(result.Message);
            return BadRequest(result);
        }
        
        return Ok(result);
    }
}