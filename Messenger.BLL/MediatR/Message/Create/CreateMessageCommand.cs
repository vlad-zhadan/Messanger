using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;

namespace Messenger.BLL.MediatR.PersonalMessage.Create;

public record CreateMessageCommand(MessageSendDto NewMessage)
    : IRequest<Result<MessageReceiveDto>>;
