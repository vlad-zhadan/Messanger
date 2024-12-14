using FluentResults;
using MediatR;
using Messenger.BLL.DTO.Profile;

namespace Messenger.BLL.MediatR.Profile;

public record GetAllProfilesByNameOrTegQuery(string NameOrTag) : IRequest<Result<IEnumerable<ProfileDto>>>;
