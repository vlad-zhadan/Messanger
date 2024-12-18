using MediatR;
using Messenger.BLL.DTO.PersonalChat;
using Messenger.BLL.MediatR.PersonalChat.Create;
using Messenger.BLL.MediatR.PersonalChat.Delete;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers.PersonalChat;

public class PersonalChatController : BaseController
{
    public PersonalChatController(IMediator mediator) : base(mediator)
    {
    }
    
    // [HttpPost()]
    // public async Task<IActionResult> PersonalChat([FromBody] )
    // {
    //     return HandleResult();
    // }

    [HttpDelete("{chatId:int}")]
    public async Task<IActionResult> DeletePersonalChat(int chatId)
    {
        return HandleResult(await _mediator.Send(new DeletePersonalChatCommand(chatId)));
    }
}