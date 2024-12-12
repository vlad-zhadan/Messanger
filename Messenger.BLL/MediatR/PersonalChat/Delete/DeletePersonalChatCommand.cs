using FluentResults;
using MediatR;

namespace Mesagger.BLL.MediatR.PersonalChat.Delete;

public record DeletePersonalChatCommand(int ChatId) : IRequest<Result<int>>;