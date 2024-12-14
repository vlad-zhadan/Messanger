using FluentResults;
using MediatR;
using Messenger.BLL.DTO.LastSeen;

namespace Messenger.BLL.MediatR.LastSeen.Get;

public record GetProfileLastSeenQuery(int PersonId) : IRequest<Result<LastSeenDto>>;