using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Chat;

namespace Messenger.BLL.MediatR.PersonalChat.GetAllForUser;

public record GetAllChatsForUserQuery() : IRequest<Result<IEnumerable<ChatDto>>>;
