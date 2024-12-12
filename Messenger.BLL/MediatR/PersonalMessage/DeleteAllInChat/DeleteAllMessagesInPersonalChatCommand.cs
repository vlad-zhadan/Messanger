using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.PersonalMessage;

public record DeleteAllMessagesInPersonalChatCommand(int ChatId) : IRequest<Result<int>>;