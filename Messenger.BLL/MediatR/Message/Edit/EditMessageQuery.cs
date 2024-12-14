using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChatMessageDTO;

namespace Mesagger.BLL.MediatR.Message.Edit;

public record EditMessageQuery(MessageEditDto MessageToEdit, int UserId) : IRequest<Result<MessageReceiveDto>>;