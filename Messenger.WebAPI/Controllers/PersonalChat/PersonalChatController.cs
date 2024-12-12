using MediatR;
using Mesagger.BLL.DTO.PersonalChat;
using Mesagger.BLL.MediatR.PersonalChat.Create;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers.PersonalChat;

public class PersonalChatController : BaseController
{
    public PersonalChatController(IMediator mediator) : base(mediator)
    {
    }
    
    [HttpPost()]
    public async Task<IActionResult> PersonalChat([FromBody] PersonalChatUsersDto personalChatUsers)
    {
        return HandleResult(await _mediator.Send(new CreatePersonalChatCommand(personalChatUsers)));
    }
}