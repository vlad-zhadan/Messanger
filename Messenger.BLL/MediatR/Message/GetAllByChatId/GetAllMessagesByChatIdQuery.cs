using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;

namespace Messenger.BLL.MediatR.PersonalMessage.GetAllByChatId;

public record GetAllMessagesByChatIdQuery(int ChatId) : IRequest<Result<IEnumerable<MessageReceiveDto>>>;