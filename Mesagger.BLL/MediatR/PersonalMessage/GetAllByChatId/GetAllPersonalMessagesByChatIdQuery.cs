using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.PersonalChatMessageDTO;

namespace Mesagger.BLL.MediatR.PersonalMessage.GetAllByChatId;

public record GetAllPersonalMessagesByChatIdQuery(int ChatId) : IRequest<Result<IEnumerable<PersonalMessageDto>>>;