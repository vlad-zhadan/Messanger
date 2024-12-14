using FluentResults;
using MediatR;
using Messenger.BLL.DTO.MessageReceiver;
using Messenger.BLL.DTO.PersonalChatMessageDTO;

namespace Messenger.BLL.MediatR.Message.GetById;

public record GetMessageByIdQuery(int MessageId) : IRequest<Result<MessageReceiveDto>>;