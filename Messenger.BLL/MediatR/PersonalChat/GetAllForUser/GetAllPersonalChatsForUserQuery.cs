using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChat;

namespace Mesagger.BLL.MediatR.PersonalChat.GetAllForUser;

public record GetAllPersonalChatsForUserQuery(int UserId) : IRequest<Result<IEnumerable<PersonalChatDto>>>;
