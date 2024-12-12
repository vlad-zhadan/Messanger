using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;

namespace Mesagger.BLL.MediatR.PersonalMessage.Create;

public record CreatePersonalMessageCommand(PersonalMessageSendDto NewMessage)
    : IRequest<Result<PersonalMessageReceiveDto>>;
