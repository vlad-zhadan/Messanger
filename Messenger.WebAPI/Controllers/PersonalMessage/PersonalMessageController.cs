using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;
using Mesagger.BLL.MediatR.PersonalMessage.GetAllByChatId;
using Messenger.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace Messenger.WebAPI.Controllers;

public class PersonalMessageController : BaseController
{
    public PersonalMessageController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetPersonalMessageAsync(int id)
    {
        return HandleResult(await _mediator.Send(new GetAllPersonalMessagesByChatIdQuery(id)));
    }
}