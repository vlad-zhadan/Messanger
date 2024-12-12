using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.LastSeen;

namespace Mesagger.BLL.MediatR.LastSeen.Get;

public record GetProfileLastSeenQuery(int PersonId) : IRequest<Result<LastSeenDto>>;