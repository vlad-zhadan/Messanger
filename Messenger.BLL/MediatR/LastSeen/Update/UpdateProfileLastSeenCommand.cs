using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.LastSeen;

namespace Mesagger.BLL.MediatR.Profile.UpdateLastSeenOnConnect;

public record UpdateProfileLastSeenCommand(LastSeenDto LastSeen) : IRequest<Result<int>>;