using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile;

public record GetAllProfilesByNameOrTegQuery(string NameOrTag) : IRequest<Result<IEnumerable<ProfileDto>>>;
