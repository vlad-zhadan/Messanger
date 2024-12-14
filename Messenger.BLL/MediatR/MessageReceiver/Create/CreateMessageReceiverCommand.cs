using FluentResults;
using MediatR;
using Messenger.BLL.DTO.MessageReceiver;

namespace Messenger.BLL.MediatR.MessageReceiver.Create;

public record CreateMessageReceiverCommand(MessageReceiverSendDto messageReceiver) : IRequest<Result<MessageReceiverReceiveDto>>;