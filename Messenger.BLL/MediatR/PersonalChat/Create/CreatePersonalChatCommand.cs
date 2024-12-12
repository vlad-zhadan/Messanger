using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChat;

namespace Mesagger.BLL.MediatR.PersonalChat.Create;

public record CreatePersonalChatCommand(PersonalChatUsersDto ChatUsers) : IRequest<Result<PersonalChatDto>>;