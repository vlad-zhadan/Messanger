using FluentResults;
using MediatR;

namespace Messenger.BLL.MediatR.PersonalMessage;

public record DeleteAllMessagesInChatCommand(int ChatId) : IRequest<Result<int>>;