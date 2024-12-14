using FluentResults;
using MediatR;
using Messenger.BLL.DTO.LastSeen;

namespace Messenger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;

public record UpdateProfileLastSeenCommand(LastSeenDto LastSeen) : IRequest<Result<int>>;