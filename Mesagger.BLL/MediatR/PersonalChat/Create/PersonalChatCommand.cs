using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChat;

namespace Mesagger.BLL.MediatR.PersonalChat.Create;

public record PersonalChatCommand(PersonalChatUsersDto ChatUsers) : IRequest<Result<PersonalChatDto>>;