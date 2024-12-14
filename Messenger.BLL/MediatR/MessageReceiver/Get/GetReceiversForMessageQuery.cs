using FluentResults;
using MediatR;
using Messenger.BLL.DTO.MessageReceiver;

namespace Messenger.BLL.MediatR.MessageReceiver.Get;

public record GetReceiversForMessageQuery(int MessageId) : IRequest<Result<IEnumerable<MessageReceiverReceiveDto>>>;