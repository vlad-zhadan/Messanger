using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;
using Messenger.BLL.MediatR.PersonalMessage;
using Messenger.BLL.MediatR.PersonalMessage.GetAllByChatId;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class PersonalMessageController : BaseController
{
    public PersonalMessageController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetPersonalMessagesAsync(int id)
    {
        return HandleResult(await _mediator.Send(new GetAllMessagesByChatIdQuery(id)));
    }
    
    [HttpDelete("{chatId:int}")]
    public async Task<ActionResult> DeleteAllMessagesInPersonalChatAsync(int chatId)
    {
        return HandleResult(await _mediator.Send(new DeleteAllMessagesInChatCommand(chatId)));
    }
}