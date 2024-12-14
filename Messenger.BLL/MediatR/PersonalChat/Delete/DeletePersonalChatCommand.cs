using FluentResults;
using MediatR;

namespace Messenger.BLL.MediatR.PersonalChat.Delete;

public record DeletePersonalChatCommand(int ChatId) : IRequest<Result<int>>;