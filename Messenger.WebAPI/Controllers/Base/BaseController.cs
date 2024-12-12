using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
public class BaseController : ControllerBase
{
    protected readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return (result.Value is null) ?
                NotFound("Not Found") : Ok(result.Value);
        }

        return BadRequest(result.Reasons);
    }
}