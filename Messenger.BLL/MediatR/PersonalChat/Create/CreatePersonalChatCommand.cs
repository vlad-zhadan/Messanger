using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChat;

namespace Messenger.BLL.MediatR.PersonalChat.Create;

public record CreatePersonalChatCommand(int SecondUserId) : IRequest<Result<PersonalChatDto>>;