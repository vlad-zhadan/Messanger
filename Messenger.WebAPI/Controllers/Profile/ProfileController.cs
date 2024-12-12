using MediatR;
using Mesagger.BLL.DTO.Profile;
using Mesagger.BLL.MediatR.LastSeen.Get;
using Mesagger.BLL.MediatR.Profile;
using Mesagger.BLL.MediatR.Profile.Create;
using Mesagger.BLL.MediatR.Profile.GetById;
using Mesagger.BLL.MediatR.Profile.Update;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers.Profile;

public class ProfileController : BaseController
{
    public ProfileController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProfile(int id)
    {
        return HandleResult(await _mediator.Send(new GetProfileByIdQuery(id)));
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> GetProfilesByNameOr(string nameOrTeg )
    {
        return HandleResult(await _mediator.Send(new GetAllProfilesByNameOrTegQuery(nameOrTeg)));
    }
    
    [HttpGet("{id:int}/lastSeen")]
    public async Task<IActionResult> GetLastSeen(int id)
    {
        return HandleResult(await _mediator.Send(new GetProfileLastSeenQuery(id)));
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateProfile([FromBody] ProfileCreateDto newProfile)
    {
        return HandleResult(await _mediator.Send(new CreateProfileCommand(newProfile)));
    }
    
    [HttpPut()]
    public async Task<IActionResult> UpdateProfile([FromBody] ProfileUpdateDto updatedProfile)
    {
        return HandleResult(await _mediator.Send(new UpdateProfileCommand(updatedProfile)));
    }
}