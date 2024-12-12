using FluentResults;
using MediatR;
using Mesagger.BLL.DTO.Profile;

namespace Mesagger.BLL.MediatR.Profile.GetById;

public record GetProfileByIdQuery(int ProfileId) : IRequest<Result<ProfileDto>>;