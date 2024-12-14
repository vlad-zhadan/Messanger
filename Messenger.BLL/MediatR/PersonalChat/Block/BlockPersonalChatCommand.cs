using FluentResults;
using MediatR;
using Messenger.BLL.DTO.PersonalChat;

namespace Mesagger.BLL.MediatR.PersonalChat.Block;

public record BlockPersonalChatCommand(int UserToBlockId, int UserId) : IRequest<Result<int>>;